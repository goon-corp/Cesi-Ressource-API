using System.Security.Claims;
using Amazon.S3.Model;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.AspNetCore.Http;
using Moq;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Features.RessourceConfidentialityTypes.Repositories;
using Ressource_API.Features.RessourceMedias.Dtos;
using Ressource_API.Features.RessourceMedias.Services;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.Query;
using Ressource_API.Features.Ressources.Repositories;
using Ressource_API.Features.Ressources.Services;
using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Features.RessourceStatuses.Repositories;
using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.RessourceTypes.Repositories;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.Repositories;
using Tag = Ressource_API.Features.Tags.Models.Tag;

namespace Ressource_API.Tests.Features.Ressources.Services;

/// <summary>
/// Minimal HybridCache implementation for unit testing.
/// HybridCache.GetOrCreateAsync is not virtual on the generic overload used by the service,
/// so Moq cannot mock it. This fake always executes the factory (no actual caching).
/// </summary>
public sealed class FakeHybridCache : HybridCache
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

public class RessourceServiceTests
{
    private readonly Mock<IRessourceRepository> _repositoryMock;
    private readonly FakeHybridCache _fakeCache;
    private readonly Mock<IRessourceMediaService> _mediaServiceMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IRessourceStatusRepository> _statusRepositoryMock;
    private readonly Mock<IRessourceConfidentialityTypeRepository> _confidentialityRepositoryMock;
    private readonly Mock<IRessourceTypeRepository> _typeRepositoryMock;
    private readonly RessourceService _sut;

    public RessourceServiceTests()
    {
        _repositoryMock = new Mock<IRessourceRepository>();
        _fakeCache = new FakeHybridCache();
        _mediaServiceMock = new Mock<IRessourceMediaService>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _statusRepositoryMock = new Mock<IRessourceStatusRepository>();
        _confidentialityRepositoryMock = new Mock<IRessourceConfidentialityTypeRepository>();
        _typeRepositoryMock = new Mock<IRessourceTypeRepository>();

        _sut = new RessourceService(
            _repositoryMock.Object,
            _fakeCache,
            _mediaServiceMock.Object,
            _tagRepositoryMock.Object,
            _statusRepositoryMock.Object,
            _confidentialityRepositoryMock.Object,
            _typeRepositoryMock.Object);
    }

    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private static ClaimsPrincipal CreateUserPrincipal(Guid userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
    }

    private static ClaimsPrincipal CreateUnauthenticatedPrincipal()
    {
        return new ClaimsPrincipal(new ClaimsIdentity());
    }

    private static RessourceStatus CreateStatus(Guid? id = null)
        => new() { Id = id ?? Guid.NewGuid(), Label = "Draft", CreationTime = DateTime.UtcNow };

    private static RessourceConfidentialityType CreateConfidentialityType(Guid? id = null)
        => new() { Id = id ?? Guid.NewGuid(), Label = "Public", CreationTime = DateTime.UtcNow };

    private static RessourceType CreateRessourceType(Guid? id = null)
        => new() { Id = id ?? Guid.NewGuid(), Label = "Article", CreationTime = DateTime.UtcNow };

    private static Ressource CreateRessource(Guid? id = null, Guid? userId = null)
    {
        return new Ressource
        {
            Id = id ?? Guid.NewGuid(),
            Title = "Test Ressource",
            Description = "Test Description",
            UserId = userId ?? Guid.NewGuid(),
            CreationTime = DateTime.UtcNow,
            RessourceStatusId = Guid.NewGuid(),
            RessourceConfidentialityTypeId = Guid.NewGuid(),
            RessourceTypeId = Guid.NewGuid(),
            RessourceStatus = CreateStatus(),
            RessourceConfidentialityType = CreateConfidentialityType(),
            RessourceType = CreateRessourceType(),
            Tags = new List<Tag>()
        };
    }

    private static CreateRessourceDto CreateCreateDto(Guid? statusId = null, Guid? confidentialityId = null,
        Guid? typeId = null)
    {
        return new CreateRessourceDto
        {
            Title = "New Ressource",
            Description = "New Description",
            StatusId = statusId ?? Guid.NewGuid(),
            ConfidentialityTypeId = confidentialityId ?? Guid.NewGuid(),
            TypeId = typeId ?? Guid.NewGuid(),
            Tags = Enumerable.Empty<Guid>()
        };
    }

    private static UpdateRessourceDto CreateUpdateDto()
    {
        return new UpdateRessourceDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Tags = new List<Guid>(),
            StatusId = Guid.NewGuid(),
            ConfidentialityTypeId = Guid.NewGuid(),
            TypeId = Guid.NewGuid()
        };
    }

    private void SetupLookupRepositories(CreateRessourceDto dto)
    {
        _statusRepositoryMock
            .Setup(r => r.FindAsync(dto.StatusId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateStatus(dto.StatusId));

        _confidentialityRepositoryMock
            .Setup(r => r.FindAsync(dto.ConfidentialityTypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateConfidentialityType(dto.ConfidentialityTypeId));

        _typeRepositoryMock
            .Setup(r => r.FindAsync(dto.TypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateRessourceType(dto.TypeId));
    }

    private void SetupLookupRepositories(UpdateRessourceDto dto)
    {
        _statusRepositoryMock
            .Setup(r => r.FindAsync(dto.StatusId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateStatus(dto.StatusId));

        _confidentialityRepositoryMock
            .Setup(r => r.FindAsync(dto.ConfidentialityTypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateConfidentialityType(dto.ConfidentialityTypeId));

        _typeRepositoryMock
            .Setup(r => r.FindAsync(dto.TypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateRessourceType(dto.TypeId));
    }

    // ──────────────────────────────────────────────
    //  CreateRessourceAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task CreateRessourceAsync_ReturnsCreatedRessource()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = CreateUserPrincipal(userId);
        var dto = CreateCreateDto();
        SetupLookupRepositories(dto);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Ressource>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ressource r, CancellationToken _) => r);

        // Act
        var result = await _sut.CreateRessourceAsync(dto, user);

        // Assert
        Assert.Equal("New Ressource", result.Title);
        Assert.Equal("New Description", result.Description);
    }

    [Fact]
    public async Task CreateRessourceAsync_CallsRepositoryAddAsync()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = CreateUserPrincipal(userId);
        var dto = CreateCreateDto();
        SetupLookupRepositories(dto);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Ressource>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ressource r, CancellationToken _) => r);

        // Act
        await _sut.CreateRessourceAsync(dto, user);

        // Assert
        _repositoryMock.Verify(
            r => r.AddAsync(
                It.Is<Ressource>(res => res.Title == "New Ressource" && res.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateRessourceAsync_WhenNoAuthor_ThrowsNullReferenceException()
    {
        // Arrange
        var user = CreateUnauthenticatedPrincipal();
        var dto = CreateCreateDto();

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(
            () => _sut.CreateRessourceAsync(dto, user));
    }

    [Fact]
    public async Task CreateRessourceAsync_InvalidatesIncompletePageCache()
    {
        // Arrange
        var user = CreateUserPrincipal(Guid.NewGuid());
        var dto = CreateCreateDto();
        SetupLookupRepositories(dto);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Ressource>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ressource r, CancellationToken _) => r);

        // Act
        await _sut.CreateRessourceAsync(dto, user);

        // Assert
        Assert.Contains("ressources:incompletePage", _fakeCache.RemovedTags);
    }

    [Fact]
    public async Task CreateRessourceAsync_WithThumbnail_CallsMediaService()
    {
        // Arrange
        var user = CreateUserPrincipal(Guid.NewGuid());
        var thumbnailMock = new Mock<IFormFile>();
        var dto = CreateCreateDto();
        dto.Thumbnail = thumbnailMock.Object;
        SetupLookupRepositories(dto);

        var mediaId = Guid.NewGuid();
        _mediaServiceMock
            .Setup(m => m.CreateMedia(It.IsAny<CreateRessourceMediaDto>()))
            .ReturnsAsync(new ReturnRessourceMediaDto
            {
                Id = mediaId,
                MediaUrl = "https://example.com/img.jpg",
                MimeType = "image/jpeg"
            });

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Ressource>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ressource r, CancellationToken _) => r);

        // Act
        await _sut.CreateRessourceAsync(dto, user);

        // Assert
        _mediaServiceMock.Verify(
            m => m.CreateMedia(It.IsAny<CreateRessourceMediaDto>()),
            Times.Once);

        _repositoryMock.Verify(
            r => r.AddAsync(
                It.Is<Ressource>(res => res.ThumbnailId == mediaId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateRessourceAsync_WithoutThumbnail_DoesNotCallMediaService()
    {
        // Arrange
        var user = CreateUserPrincipal(Guid.NewGuid());
        var dto = CreateCreateDto();
        dto.Thumbnail = null;
        SetupLookupRepositories(dto);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Ressource>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ressource r, CancellationToken _) => r);

        // Act
        await _sut.CreateRessourceAsync(dto, user);

        // Assert
        _mediaServiceMock.Verify(
            m => m.CreateMedia(It.IsAny<CreateRessourceMediaDto>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateRessourceAsync_WithTags_FetchesTagsFromRepository()
    {
        // Arrange
        var user = CreateUserPrincipal(Guid.NewGuid());
        var tagId1 = Guid.NewGuid();
        var tagId2 = Guid.NewGuid();
        var dto = CreateCreateDto();
        dto.Tags = new List<Guid> { tagId1, tagId2 };
        SetupLookupRepositories(dto);

        var tags = new List<Tag>
        {
            new() { Id = tagId1, Label = "Tag1", CreationTime = DateTime.UtcNow },
            new() { Id = tagId2, Label = "Tag2", CreationTime = DateTime.UtcNow }
        };

        _tagRepositoryMock
            .Setup(r => r.ListAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Tag, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Ressource>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ressource r, CancellationToken _) => r);

        // Act
        await _sut.CreateRessourceAsync(dto, user);

        // Assert
        _repositoryMock.Verify(
            r => r.AddAsync(
                It.Is<Ressource>(res => res.Tags.Count == 2),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ──────────────────────────────────────────────
    //  UpdateRessourceAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task UpdateRessourceAsync_WhenExists_ReturnsUpdatedRessource()
    {
        // Arrange
        var ressource = CreateRessource();
        var dto = CreateUpdateDto();
        SetupLookupRepositories(dto);

        _repositoryMock
            .Setup(r => r.FindWithTagsAsync(ressource.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ressource);

        // Act
        var result = await _sut.UpdateRessourceAsync(ressource.Id, dto);

        // Assert
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Updated Description", result.Description);
    }

    [Fact]
    public async Task UpdateRessourceAsync_WhenExists_CallsRepositoryUpdateAsync()
    {
        // Arrange
        var ressource = CreateRessource();
        var dto = CreateUpdateDto();
        SetupLookupRepositories(dto);

        _repositoryMock
            .Setup(r => r.FindWithTagsAsync(ressource.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ressource);

        // Act
        await _sut.UpdateRessourceAsync(ressource.Id, dto);

        // Assert
        _repositoryMock.Verify(
            r => r.UpdateAsync(
                It.Is<Ressource>(res => res.Title == "Updated Title"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateRessourceAsync_WhenNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = CreateUpdateDto();

        _repositoryMock
            .Setup(r => r.FindWithTagsAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ressource?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _sut.UpdateRessourceAsync(id, dto));
    }

    [Fact]
    public async Task UpdateRessourceAsync_SetsUpdateTime()
    {
        // Arrange
        var ressource = CreateRessource();
        var dto = CreateUpdateDto();
        var beforeUpdate = DateTime.UtcNow;
        SetupLookupRepositories(dto);

        _repositoryMock
            .Setup(r => r.FindWithTagsAsync(ressource.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ressource);

        // Act
        await _sut.UpdateRessourceAsync(ressource.Id, dto);

        // Assert
        Assert.NotNull(ressource.UpdateTime);
        Assert.True(ressource.UpdateTime >= beforeUpdate);
    }

    [Fact]
    public async Task UpdateRessourceAsync_RemovesCacheEntry()
    {
        // Arrange
        var ressource = CreateRessource();
        var dto = CreateUpdateDto();
        SetupLookupRepositories(dto);

        _repositoryMock
            .Setup(r => r.FindWithTagsAsync(ressource.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ressource);

        // Act
        await _sut.UpdateRessourceAsync(ressource.Id, dto);

        // Assert
        Assert.Contains($"ressources:{ressource.Id}", _fakeCache.RemovedKeys);
    }

    // ──────────────────────────────────────────────
    //  DeleteRessourceAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task DeleteRessourceAsync_WhenExists_ReturnsTrue()
    {
        // Arrange
        var ressource = CreateRessource();

        _repositoryMock
            .Setup(r => r.FindAsync(ressource.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ressource);

        // Act
        var result = await _sut.DeleteRessourceAsync(ressource.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteRessourceAsync_WhenExists_CallsRepositoryDeleteAsync()
    {
        // Arrange
        var ressource = CreateRessource();

        _repositoryMock
            .Setup(r => r.FindAsync(ressource.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ressource);

        // Act
        await _sut.DeleteRessourceAsync(ressource.Id);

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteAsync(ressource, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteRessourceAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ressource?)null);

        // Act
        var result = await _sut.DeleteRessourceAsync(id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteRessourceAsync_WhenNotFound_DoesNotCallDeleteAsync()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ressource?)null);

        // Act
        await _sut.DeleteRessourceAsync(id);

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteAsync(It.IsAny<Ressource>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteRessourceAsync_RemovesCacheEntries()
    {
        // Arrange
        var ressource = CreateRessource();

        _repositoryMock
            .Setup(r => r.FindAsync(ressource.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ressource);

        // Act
        await _sut.DeleteRessourceAsync(ressource.Id);

        // Assert
        Assert.Contains($"ressources:{ressource.Id}", _fakeCache.RemovedKeys);
        Assert.Contains($"invert:ressources:{ressource.Id}", _fakeCache.RemovedKeys);
    }

    // ──────────────────────────────────────────────
    //  LikeRessource
    // ──────────────────────────────────────────────

    [Fact]
    public async Task LikeRessource_WhenAuthenticated_ReturnsSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ressourceId = Guid.NewGuid();
        var user = CreateUserPrincipal(userId);

        _repositoryMock
            .Setup(r => r.ToggleLikeAsync(ressourceId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.LikeRessource(ressourceId, user);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task LikeRessource_WhenNotAuthenticated_ReturnsFailure()
    {
        // Arrange
        var ressourceId = Guid.NewGuid();
        var user = CreateUnauthenticatedPrincipal();

        // Act
        var result = await _sut.LikeRessource(ressourceId, user);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task LikeRessource_WhenRessourceNotFound_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ressourceId = Guid.NewGuid();
        var user = CreateUserPrincipal(userId);

        _repositoryMock
            .Setup(r => r.ToggleLikeAsync(ressourceId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((bool?)null);

        // Act
        var result = await _sut.LikeRessource(ressourceId, user);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task LikeRessource_WhenNotAuthenticated_DoesNotCallRepository()
    {
        // Arrange
        var user = CreateUnauthenticatedPrincipal();

        // Act
        await _sut.LikeRessource(Guid.NewGuid(), user);

        // Assert
        _repositoryMock.Verify(
            r => r.ToggleLikeAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // ──────────────────────────────────────────────
    //  FavoriteRessource
    // ──────────────────────────────────────────────

    [Fact]
    public async Task FavoriteRessource_WhenAuthenticated_ReturnsSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ressourceId = Guid.NewGuid();
        var user = CreateUserPrincipal(userId);

        _repositoryMock
            .Setup(r => r.ToggleFavoriteAsync(ressourceId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.FavoriteRessource(ressourceId, user);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task FavoriteRessource_WhenNotAuthenticated_ReturnsFailure()
    {
        // Arrange
        var user = CreateUnauthenticatedPrincipal();

        // Act
        var result = await _sut.FavoriteRessource(Guid.NewGuid(), user);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task FavoriteRessource_WhenRessourceNotFound_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ressourceId = Guid.NewGuid();
        var user = CreateUserPrincipal(userId);

        _repositoryMock
            .Setup(r => r.ToggleFavoriteAsync(ressourceId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((bool?)null);

        // Act
        var result = await _sut.FavoriteRessource(ressourceId, user);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task FavoriteRessource_WhenNotAuthenticated_DoesNotCallRepository()
    {
        // Arrange
        var user = CreateUnauthenticatedPrincipal();

        // Act
        await _sut.FavoriteRessource(Guid.NewGuid(), user);

        // Assert
        _repositoryMock.Verify(
            r => r.ToggleFavoriteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // ──────────────────────────────────────────────
    //  GetAllRessourcesAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetAllRessourcesAsync_WithRessourceTypeFilter_BypassesCache()
    {
        // Arrange
        var query = new RessourceQuery { page = 1, size = 10, RessourceType = "Article" };
        var paginatedResult = new PaginatedList<ReturnRessourceDto>(new List<ReturnRessourceDto>(), 1, 10, 0);

        _repositoryMock
            .Setup(r => r.PaginatedRessourcesAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        // Act
        var result = await _sut.GetAllRessourcesAsync(query);

        // Assert
        _repositoryMock.Verify(
            r => r.PaginatedRessourcesAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllRessourcesAsync_WithTagsFilter_BypassesCache()
    {
        // Arrange
        var query = new RessourceQuery
        {
            page = 1, size = 10,
            RessourceTags = new List<Guid> { Guid.NewGuid() }
        };
        var paginatedResult = new PaginatedList<ReturnRessourceDto>(new List<ReturnRessourceDto>(), 1, 10, 0);

        _repositoryMock
            .Setup(r => r.PaginatedRessourcesAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        // Act
        var result = await _sut.GetAllRessourcesAsync(query);

        // Assert
        _repositoryMock.Verify(
            r => r.PaginatedRessourcesAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllRessourcesAsync_WithTitleFilter_BypassesCache()
    {
        // Arrange
        var query = new RessourceQuery { page = 1, size = 10, RessourceTitle = "search" };
        var paginatedResult = new PaginatedList<ReturnRessourceDto>(new List<ReturnRessourceDto>(), 1, 10, 0);

        _repositoryMock
            .Setup(r => r.PaginatedRessourcesAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        // Act
        var result = await _sut.GetAllRessourcesAsync(query);

        // Assert
        _repositoryMock.Verify(
            r => r.PaginatedRessourcesAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllRessourcesAsync_WithIsDeletedFilter_BypassesCache()
    {
        // Arrange
        var query = new RessourceQuery { page = 1, size = 10, IsDeleted = false };
        var paginatedResult = new PaginatedList<ReturnRessourceDto>(new List<ReturnRessourceDto>(), 1, 10, 0);

        _repositoryMock
            .Setup(r => r.PaginatedRessourcesAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        // Act
        var result = await _sut.GetAllRessourcesAsync(query);

        // Assert
        _repositoryMock.Verify(
            r => r.PaginatedRessourcesAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}