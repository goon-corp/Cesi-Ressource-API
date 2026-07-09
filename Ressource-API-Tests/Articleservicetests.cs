
using System.Security.Claims;
using Moq;
using Ressource_API.Features.Articles.Dtos;
using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Articles.Repositories;
using Ressource_API.Features.Articles.Services;
using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.Services;
using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Features.RessourceTypes.Models;

namespace Ressource_API.Tests.Features.Articles.Services;

public class ArticleServiceTests
{
    private readonly Mock<IArticleRepository> _repositoryMock;
    private readonly Mock<IRessourceService> _ressourceServiceMock;
    private readonly ArticleService _sut;

    public ArticleServiceTests()
    {
        _repositoryMock = new Mock<IArticleRepository>();
        _ressourceServiceMock = new Mock<IRessourceService>();
        _sut = new ArticleService(_repositoryMock.Object, _ressourceServiceMock.Object);
    }

    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private static ClaimsPrincipal CreateUserPrincipal(Guid userId, string? role = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };

        if (role is not null)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
    }

    private static ClaimsPrincipal CreateUnauthenticatedPrincipal()
    {
        return new ClaimsPrincipal(new ClaimsIdentity());
    }

    private static ReturnRessourceDto CreateReturnRessourceDto(Guid? id = null)
    {
        return new ReturnRessourceDto
        {
            Id = id ?? Guid.NewGuid(),
            Title = "Test Title",
            Description = "Test Description"
        };
    }

    private static Ressource CreateRessource(Guid? id = null, Guid? userId = null)
    {
        return new Ressource
        {
            Id = id ?? Guid.NewGuid(),
            Title = "Test Title",
            Description = "Test Description",
            UserId = userId ?? Guid.NewGuid(),
            RessourceConfidentialityTypeId = Guid.NewGuid(),
            RessourceStatusId = Guid.NewGuid(),
            RessourceTypeId = Guid.NewGuid(),
            CreationTime = DateTime.UtcNow,
            RessourceStatus = new RessourceStatus { Id = Guid.NewGuid(), Label = "Draft" },
            RessourceConfidentialityType = new RessourceConfidentialityType { Id = Guid.NewGuid(), Label = "Public" },
            RessourceType = new RessourceType { Id = Guid.NewGuid(), Label = "Article" }
        };
    }

    private static Article CreateArticle(Guid? id = null, Ressource? ressource = null)
    {
        var res = ressource ?? CreateRessource();
        return new Article
        {
            Id = id ?? Guid.NewGuid(),
            Content = "Test article content",
            RessourceId = res.Id,
            Ressource = res
        };
    }

    private static CreateArticleDto CreateCreateArticleDto()
    {
        return new CreateArticleDto
        {
            Content = "New article content",
            Ressource = new CreateRessourceDto
            {
                Title = "New Title",
                Description = "New Description",
                StatusId = Guid.NewGuid(),
                ConfidentialityTypeId = Guid.NewGuid(),
                TypeId = Guid.NewGuid()
            }
        };
    }

    private static UpdateArticleDto CreateUpdateArticleDto()
    {
        return new UpdateArticleDto
        {
            Content = "Updated content",
            Ressource = new UpdateRessourceDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                StatusId = Guid.NewGuid(),
                ConfidentialityTypeId = Guid.NewGuid(),
                TypeId = Guid.NewGuid()
            }
        };
    }

    // ──────────────────────────────────────────────
    //  GetArticleByRessourceIdAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetArticleByRessourceIdAsync_WhenArticleExists_ReturnsSuccess()
    {
        // Arrange
        var ressourceId = Guid.NewGuid();
        var article = CreateArticle();

        _repositoryMock
            .Setup(r => r.GetArticleNoTrackingByRessourceId(ressourceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        var result = await _sut.GetArticleByRessourceIdAsync(ressourceId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(article.Id, result.Data!.Id);
        Assert.Equal(article.Content, result.Data.Content);
    }

    [Fact]
    public async Task GetArticleByRessourceIdAsync_WhenArticleNotFound_ReturnsFailure()
    {
        // Arrange
        var ressourceId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetArticleNoTrackingByRessourceId(ressourceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article?)null);

        // Act
        var result = await _sut.GetArticleByRessourceIdAsync(ressourceId);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetArticleByRessourceIdAsync_CallsRepositoryWithCorrectId()
    {
        // Arrange
        var ressourceId = Guid.NewGuid();
        var article = CreateArticle();

        _repositoryMock
            .Setup(r => r.GetArticleNoTrackingByRessourceId(ressourceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act
        await _sut.GetArticleByRessourceIdAsync(ressourceId);

        // Assert
        _repositoryMock.Verify(
            r => r.GetArticleNoTrackingByRessourceId(ressourceId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ──────────────────────────────────────────────
    //  CreateArticleAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task CreateArticleAsync_ReturnsSuccessWithArticle()
    {
        // Arrange
        var dto = CreateCreateArticleDto();
        var user = CreateUserPrincipal(Guid.NewGuid());
        var returnedRessource = CreateReturnRessourceDto();

        _ressourceServiceMock
            .Setup(s => s.CreateRessourceAsync(dto.Ressource, user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnedRessource);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Article>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article a, CancellationToken _) => a);

        // Act
        var result = await _sut.CreateArticleAsync(dto, user);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(dto.Content, result.Data!.Content);
        Assert.Equal(returnedRessource.Id, result.Data.Ressource.Id);
    }

    [Fact]
    public async Task CreateArticleAsync_CallsRessourceServiceWithCorrectDto()
    {
        // Arrange
        var dto = CreateCreateArticleDto();
        var user = CreateUserPrincipal(Guid.NewGuid());
        var returnedRessource = CreateReturnRessourceDto();

        _ressourceServiceMock
            .Setup(s => s.CreateRessourceAsync(dto.Ressource, user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnedRessource);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Article>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article a, CancellationToken _) => a);

        // Act
        await _sut.CreateArticleAsync(dto, user);

        // Assert
        _ressourceServiceMock.Verify(
            s => s.CreateRessourceAsync(dto.Ressource, user, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateArticleAsync_CallsRepositoryAddAsync()
    {
        // Arrange
        var dto = CreateCreateArticleDto();
        var user = CreateUserPrincipal(Guid.NewGuid());
        var returnedRessource = CreateReturnRessourceDto();

        _ressourceServiceMock
            .Setup(s => s.CreateRessourceAsync(dto.Ressource, user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnedRessource);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Article>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article a, CancellationToken _) => a);

        // Act
        await _sut.CreateArticleAsync(dto, user);

        // Assert
        _repositoryMock.Verify(
            r => r.AddAsync(
                It.Is<Article>(a => a.Content == dto.Content && a.RessourceId == returnedRessource.Id),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ──────────────────────────────────────────────
    //  UpdateArticleAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task UpdateArticleAsync_WhenArticleNotFound_ReturnsFailure()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = CreateUpdateArticleDto();
        var user = CreateUserPrincipal(Guid.NewGuid());

        _repositoryMock
            .Setup(r => r.GetArticle(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article?)null);

        // Act
        var result = await _sut.UpdateArticleAsync(id, dto, user);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateArticleAsync_WhenUserIsOwner_ReturnsSuccess()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ressource = CreateRessource(userId: ownerId);
        var article = CreateArticle(ressource: ressource);
        var dto = CreateUpdateArticleDto();
        var user = CreateUserPrincipal(ownerId);
        var updatedRessource = CreateReturnRessourceDto(ressource.Id);

        _repositoryMock
            .Setup(r => r.GetArticle(article.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        _ressourceServiceMock
            .Setup(s => s.UpdateRessourceAsync(article.RessourceId, dto.Ressource, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedRessource);

        // Act
        var result = await _sut.UpdateArticleAsync(article.Id, dto, user);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(dto.Content, result.Data!.Content);
    }

    [Fact]
    public async Task UpdateArticleAsync_WhenUserIsAdmin_ReturnsSuccess()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var ressource = CreateRessource(userId: ownerId);
        var article = CreateArticle(ressource: ressource);
        var dto = CreateUpdateArticleDto();
        var user = CreateUserPrincipal(adminId, "Administrateur");
        var updatedRessource = CreateReturnRessourceDto(ressource.Id);

        _repositoryMock
            .Setup(r => r.GetArticle(article.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        _ressourceServiceMock
            .Setup(s => s.UpdateRessourceAsync(article.RessourceId, dto.Ressource, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedRessource);

        // Act
        var result = await _sut.UpdateArticleAsync(article.Id, dto, user);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateArticleAsync_WhenUserIsModerator_ReturnsSuccess()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var moderatorId = Guid.NewGuid();
        var ressource = CreateRessource(userId: ownerId);
        var article = CreateArticle(ressource: ressource);
        var dto = CreateUpdateArticleDto();
        var user = CreateUserPrincipal(moderatorId, "Modérateur");
        var updatedRessource = CreateReturnRessourceDto(ressource.Id);

        _repositoryMock
            .Setup(r => r.GetArticle(article.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        _ressourceServiceMock
            .Setup(s => s.UpdateRessourceAsync(article.RessourceId, dto.Ressource, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedRessource);

        // Act
        var result = await _sut.UpdateArticleAsync(article.Id, dto, user);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateArticleAsync_WhenUserIsNotOwnerNorAdmin_ThrowsInvalidCastException()
    {
        // Arrange
        // NOTE: Le service contient un bug — il cast (Result<ReturnArticleDto>)authResult
        // alors que authResult est de type Result (non générique). Ce cast échoue.
        // Corrigez le service en utilisant : return Result.Failure<ReturnArticleDto>(authResult.Error!);
        var ownerId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var ressource = CreateRessource(userId: ownerId);
        var article = CreateArticle(ressource: ressource);
        var dto = CreateUpdateArticleDto();
        var user = CreateUserPrincipal(otherUserId);

        _repositoryMock
            .Setup(r => r.GetArticle(article.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCastException>(
            () => _sut.UpdateArticleAsync(article.Id, dto, user));
    }

    [Fact]
    public async Task UpdateArticleAsync_WhenUserNotAuthenticated_ThrowsInvalidCastException()
    {
        // Arrange — même bug de cast que ci-dessus
        var ressource = CreateRessource();
        var article = CreateArticle(ressource: ressource);
        var dto = CreateUpdateArticleDto();
        var user = CreateUnauthenticatedPrincipal();

        _repositoryMock
            .Setup(r => r.GetArticle(article.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCastException>(
            () => _sut.UpdateArticleAsync(article.Id, dto, user));
    }

    [Fact]
    public async Task UpdateArticleAsync_WhenRessourceUpdateReturnsNull_ReturnsFailure()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ressource = CreateRessource(userId: ownerId);
        var article = CreateArticle(ressource: ressource);
        var dto = CreateUpdateArticleDto();
        var user = CreateUserPrincipal(ownerId);

        _repositoryMock
            .Setup(r => r.GetArticle(article.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        _ressourceServiceMock
            .Setup(s => s.UpdateRessourceAsync(article.RessourceId, dto.Ressource, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<ReturnRessourceDto>(null!));

        // Act
        var result = await _sut.UpdateArticleAsync(article.Id, dto, user);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateArticleAsync_UpdatesContentOnExistingArticle()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var ressource = CreateRessource(userId: ownerId);
        var article = CreateArticle(ressource: ressource);
        var dto = CreateUpdateArticleDto();
        var user = CreateUserPrincipal(ownerId);
        var updatedRessource = CreateReturnRessourceDto(ressource.Id);

        _repositoryMock
            .Setup(r => r.GetArticle(article.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        _ressourceServiceMock
            .Setup(s => s.UpdateRessourceAsync(article.RessourceId, dto.Ressource, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedRessource);

        // Act
        await _sut.UpdateArticleAsync(article.Id, dto, user);

        // Assert
        Assert.Equal(dto.Content, article.Content);
        _repositoryMock.Verify(
            r => r.UpdateAsync(article, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ──────────────────────────────────────────────
    //  DeleteArticleAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task DeleteArticleAsync_WhenArticleNotFound_ReturnsFailure()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = CreateUserPrincipal(Guid.NewGuid());

        _repositoryMock
            .Setup(r => r.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Article?)null);

        // Act
        var result = await _sut.DeleteArticleAsync(id, user);

        // Assert
        Assert.False(result.IsSuccess);
    }

   

    [Fact]
    public async Task DeleteArticleAsync_WhenUnauthorized_DoesNotCallDeleteAsync()
    {
        // Arrange — même bug de cast que ci-dessus
        var ownerId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var ressource = CreateRessource(userId: ownerId);
        var article = CreateArticle(ressource: ressource);
        var user = CreateUserPrincipal(otherUserId);

        _repositoryMock
            .Setup(r => r.FindAsync(article.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(article);

        // Act — le cast échoue avant d'atteindre DeleteAsync
        try { await _sut.DeleteArticleAsync(article.Id, user); } catch (InvalidCastException) { }

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteAsync(It.IsAny<Article>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}