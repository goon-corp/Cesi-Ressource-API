using Amazon.S3.Model;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Moq;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.Query;
using Ressource_API.Features.Tags.Repositories;
using Ressource_API.Features.Tags.Services;
using Ressource_API.Features.Tags.TagDtos;
using Tag = Ressource_API.Features.Tags.Models.Tag;

namespace Ressource_API.Tests.Features.Tags.Services;

/// <summary>
/// Minimal HybridCache implementation for unit testing.
/// HybridCache.GetOrCreateAsync is not virtual on the generic overload,
/// so Moq cannot mock it. This fake always executes the factory (no caching).
/// </summary>
public sealed class FakeHybridCacheForTags : HybridCache
{
    public readonly List<string> RemovedKeys = [];
    public readonly List<string> RemovedTags = [];

    public override ValueTask<T> GetOrCreateAsync<TState, T>(string key, TState state, Func<TState, CancellationToken, ValueTask<T>> factory, HybridCacheEntryOptions? options = null,
        IEnumerable<string>? tags = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return factory(state, cancellationToken);
    }

    // public ValueTask<T> GetOrCreateAsync<T>(
    //     string key,
    //     Func<CancellationToken, ValueTask<T>> factory,
    //     HybridCacheEntryOptions? options = null,
    //     IEnumerable<string>? tags = null,
    //     CancellationToken cancellationToken = default)
    // {
    //     
    // }

    public override ValueTask SetAsync<T>(
        string key, T value,
        HybridCacheEntryOptions? options = null,
        IEnumerable<string>? tags = null,
        CancellationToken cancellationToken = default)
    {
        return ValueTask.CompletedTask;
    }

    public override ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        RemovedKeys.Add(key);
        return ValueTask.CompletedTask;
    }

    public override ValueTask RemoveByTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        RemovedTags.Add(tag);
        return ValueTask.CompletedTask;
    }
}

public class TagServiceTests
{
    private readonly Mock<ITagRepository> _repositoryMock;
    private readonly FakeHybridCacheForTags _fakeCache;
    private readonly Mock<ILogger<TagService>> _loggerMock;
    private readonly TagService _sut;

    public TagServiceTests()
    {
        _repositoryMock = new Mock<ITagRepository>();
        _fakeCache = new FakeHybridCacheForTags();
        _loggerMock = new Mock<ILogger<TagService>>();
        _sut = new TagService(_repositoryMock.Object, _fakeCache, _loggerMock.Object);
    }

    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private static Tag CreateTag(Guid? id = null, string label = "Test Tag")
    {
        return new Tag
        {
            Id = id ?? Guid.NewGuid(),
            Label = label,
            CreationTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow,
            DeletionTime = null
        };
    }

    private static TagQuery CreateTagQuery(int page = 1, int size = 10, string? tagName = null,
        bool? isDeleted = null, DateTime? createdAt = null)
    {
        return new TagQuery
        {
            page = page,
            size = size,
            TagName = tagName,
            IsDeleted = isDeleted,
            CreatedAt = createdAt
        };
    }

    private static PaginatedList<Tag> CreatePaginatedTags(int count = 3, int page = 1, int size = 10)
    {
        var tags = Enumerable.Range(0, count)
            .Select(_ => CreateTag())
            .ToList();

        return new PaginatedList<Tag>(tags, page, size, count);
    }

    // ──────────────────────────────────────────────
    //  GetTagByIdAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetTagByIdAsync_WhenTagExists_ReturnsTag()
    {
        // Arrange
        var tag = CreateTag();

        _repositoryMock
            .Setup(r => r.FirstOrDefaultAsyncAsNoTracking(
                It.IsAny<System.Linq.Expressions.Expression<Func<Tag, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        var result = await _sut.GetTagByIdAsync(tag.Id);

        // Assert
        Assert.Equal(tag.Id, result.Id);
        Assert.Equal(tag.Label, result.Label);
    }

    [Fact]
    public async Task GetTagByIdAsync_WhenTagNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.FirstOrDefaultAsyncAsNoTracking(
                It.IsAny<System.Linq.Expressions.Expression<Func<Tag, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _sut.GetTagByIdAsync(id));
    }

    // ──────────────────────────────────────────────
    //  CreateTagAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task CreateTagAsync_ReturnsCreatedTag()
    {
        // Arrange
        var dto = new CreateTagDto { Label = "New Tag" };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag t, CancellationToken _) => t);

        // Act
        var result = await _sut.CreateTagAsync(dto);

        // Assert
        Assert.Equal("New Tag", result.Label);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task CreateTagAsync_CallsRepositoryAddAsync()
    {
        // Arrange
        var dto = new CreateTagDto { Label = "New Tag" };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag t, CancellationToken _) => t);

        // Act
        await _sut.CreateTagAsync(dto);

        // Assert
        _repositoryMock.Verify(
            r => r.AddAsync(
                It.Is<Tag>(t => t.Label == "New Tag"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateTagAsync_InvalidatesIncompletePageCache()
    {
        // Arrange
        var dto = new CreateTagDto { Label = "Tag" };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag t, CancellationToken _) => t);

        // Act
        await _sut.CreateTagAsync(dto);

        // Assert
        Assert.Contains("tags:incompletePage", _fakeCache.RemovedTags);
    }

    // ──────────────────────────────────────────────
    //  UpdateTagAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task UpdateTagAsync_WhenTagExists_ReturnsUpdatedTag()
    {
        // Arrange
        var tag = CreateTag(label: "Old Label");
        var dto = new UpdateTagDto { Label = "New Label", DeletionTime = null };

        _repositoryMock
            .Setup(r => r.FindAsync(tag.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        var result = await _sut.UpdateTagAsync(tag.Id, dto);

        // Assert
        Assert.Equal("New Label", result.Label);
        Assert.Null(result.DeletionTime);
    }

    [Fact]
    public async Task UpdateTagAsync_WhenTagExists_CallsRepositoryUpdateAsync()
    {
        // Arrange
        var tag = CreateTag();
        var dto = new UpdateTagDto { Label = "Updated" };

        _repositoryMock
            .Setup(r => r.FindAsync(tag.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        await _sut.UpdateTagAsync(tag.Id, dto);

        // Assert
        _repositoryMock.Verify(
            r => r.UpdateAsync(
                It.Is<Tag>(t => t.Label == "Updated"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateTagAsync_WhenTagNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateTagDto { Label = "Whatever" };

        _repositoryMock
            .Setup(r => r.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _sut.UpdateTagAsync(id, dto));
    }

    [Fact]
    public async Task UpdateTagAsync_SetsDeletionTime()
    {
        // Arrange
        var tag = CreateTag();
        var deletionTime = DateTime.UtcNow;
        var dto = new UpdateTagDto { Label = "Soft Delete", DeletionTime = deletionTime };

        _repositoryMock
            .Setup(r => r.FindAsync(tag.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        var result = await _sut.UpdateTagAsync(tag.Id, dto);

        // Assert
        Assert.Equal(deletionTime, result.DeletionTime);
    }

    [Fact]
    public async Task UpdateTagAsync_SetsUpdateTime()
    {
        // Arrange
        var tag = CreateTag();
        var beforeUpdate = DateTime.UtcNow;
        var dto = new UpdateTagDto { Label = "Updated" };

        _repositoryMock
            .Setup(r => r.FindAsync(tag.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        var result = await _sut.UpdateTagAsync(tag.Id, dto);

        // Assert
        Assert.NotNull(result.UpdateTime);
        Assert.True(result.UpdateTime >= beforeUpdate);
    }

    // ──────────────────────────────────────────────
    //  DeleteTagAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task DeleteTagAsync_WhenTagExists_CallsSoftDeleteAsync()
    {
        // Arrange
        var tag = CreateTag();

        _repositoryMock
            .Setup(r => r.FindAsync(tag.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        await _sut.DeleteTagAsync(tag.Id);

        // Assert
        _repositoryMock.Verify(
            r => r.SoftDeleteAsync(
                It.Is<Tag>(t => t.Id == tag.Id),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteTagAsync_WhenTagExists_SetsDeletionTime()
    {
        // Arrange
        var tag = CreateTag();
        var beforeDelete = DateTime.UtcNow;

        _repositoryMock
            .Setup(r => r.FindAsync(tag.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        await _sut.DeleteTagAsync(tag.Id);

        // Assert
        Assert.NotNull(tag.DeletionTime);
        Assert.True(tag.DeletionTime >= beforeDelete);
    }

    [Fact]
    public async Task DeleteTagAsync_WhenTagNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _sut.DeleteTagAsync(id));
    }

    [Fact]
    public async Task DeleteTagAsync_WhenTagNotFound_DoesNotCallSoftDelete()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag?)null);

        // Act
        try { await _sut.DeleteTagAsync(id); } catch (KeyNotFoundException) { }

        // Assert
        _repositoryMock.Verify(
            r => r.SoftDeleteAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteTagAsync_RemovesTagFromCache()
    {
        // Arrange
        var tag = CreateTag();

        _repositoryMock
            .Setup(r => r.FindAsync(tag.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        await _sut.DeleteTagAsync(tag.Id);

        // Assert
        Assert.Contains($"tags:{tag.Id}", _fakeCache.RemovedKeys);
    }

    [Fact]
    public async Task DeleteTagAsync_RemovesInvertedIndexFromCache()
    {
        // Arrange
        var tag = CreateTag();

        _repositoryMock
            .Setup(r => r.FindAsync(tag.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        await _sut.DeleteTagAsync(tag.Id);

        // Assert
        Assert.Contains($"invert:tags:{tag.Id}", _fakeCache.RemovedKeys);
    }

    // ──────────────────────────────────────────────
    //  GetAllTagsAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetAllTagsAsync_WithFilterTagName_BypassesCacheAndCallsRepository()
    {
        // Arrange
        var query = CreateTagQuery(tagName: "search");
        var paginatedTags = CreatePaginatedTags(2);

        _repositoryMock
            .Setup(r => r.PaginatedListAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedTags);

        // Act
        var result = await _sut.GetAllTagsAsync(query);

        // Assert
        Assert.Equal(2, result.Items.Count);
        _repositoryMock.Verify(
            r => r.PaginatedListAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllTagsAsync_WithFilterCreatedAt_BypassesCacheAndCallsRepository()
    {
        // Arrange
        var query = CreateTagQuery(createdAt: DateTime.UtcNow);
        var paginatedTags = CreatePaginatedTags(1);

        _repositoryMock
            .Setup(r => r.PaginatedListAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedTags);

        // Act
        var result = await _sut.GetAllTagsAsync(query);

        // Assert
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task GetAllTagsAsync_WithFilterIsDeleted_BypassesCacheAndCallsRepository()
    {
        // Arrange
        var query = CreateTagQuery(isDeleted: false);
        var paginatedTags = CreatePaginatedTags(5);

        _repositoryMock
            .Setup(r => r.PaginatedListAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedTags);

        // Act
        var result = await _sut.GetAllTagsAsync(query);

        // Assert
        Assert.Equal(5, result.Items.Count);
    }
}