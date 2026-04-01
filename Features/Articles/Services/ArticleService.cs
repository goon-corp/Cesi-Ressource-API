using System.Security.Claims;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Common.Utils;
using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Articles.Dtos;
using Ressource_API.Features.Articles.Extensions;
using Ressource_API.Features.Articles.Repositories;
using Ressource_API.Features.Ressources.Services;

namespace Ressource_API.Features.Articles.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _repository;
    private readonly IRessourceService _ressourceService;

    public ArticleService(IArticleRepository repository, IRessourceService ressourceService)
    {
        _repository = repository;
        _ressourceService = ressourceService;
    }

    public async Task<Result<ReturnArticleDto>> GetArticleByRessourceIdAsync(Guid ressourceId,
        CancellationToken cancellationToken = default)
    {
        var article =
            await _repository.FirstOrDefaultAsyncAsNoTracking(a => a.RessourceId == ressourceId, cancellationToken);
        if (article is null) return Result.Failure<ReturnArticleDto>("Article not found");

        return Result.Success(article.ToReturnDto());
    }

    public async Task<Result<ReturnArticleDto>> CreateArticleAsync(CreateArticleDto dto, ClaimsPrincipal context,
        CancellationToken cancellationToken = default)
    {
        var ressource = await _ressourceService.CreateRessourceAsync(dto.Ressource, context, cancellationToken);

        var article = dto.ToArticle(ressource.Id);

        await _repository.AddAsync(article, cancellationToken);
        return Result.Success(article.ToReturnDto(ressource));
    }

    public async Task<Result<ReturnArticleDto>> UpdateArticleAsync(Guid id, UpdateArticleDto dto,
        ClaimsPrincipal context, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetArticle(id, cancellationToken);
        if (existing is null) return Result.Failure<ReturnArticleDto>("Article not found");

        var authResult = AuthorizationUtils.IsAdminOrOwner(existing.Ressource.UserId, context, cancellationToken);
        if (!authResult.IsSuccess)
            return (Result<ReturnArticleDto>)authResult;

        var updatedRessource =
            await _ressourceService.UpdateRessourceAsync(existing.RessourceId, dto.Ressource, cancellationToken);
        if (updatedRessource is null) return Result.Failure<ReturnArticleDto>("Ressource not found");

        existing.Content = dto.Content;
        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(existing.ToReturnDto(updatedRessource));
    }

    public async Task<Result> DeleteArticleAsync(Guid id, ClaimsPrincipal context,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        if (existing == null) return Result.Failure<ReturnArticleDto>("Article not found");

        var authResult = AuthorizationUtils.IsAdminOrOwner(existing.Ressource.UserId, context, cancellationToken);
        if (!authResult.IsSuccess)
            return (Result<ReturnArticleDto>)authResult;

        await _repository.DeleteAsync(existing, cancellationToken);

        return Result.Success();
    }
}