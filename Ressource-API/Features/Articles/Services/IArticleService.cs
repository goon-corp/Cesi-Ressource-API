using System.Security.Claims;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Articles.Dtos;

namespace Ressource_API.Features.Articles.Services;

public interface IArticleService
{
    Task<Result<ReturnArticleDto>> GetArticleByRessourceIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<ReturnArticleDto>> CreateArticleAsync(CreateArticleDto dto, ClaimsPrincipal context,
        CancellationToken cancellationToken = default);

    Task<Result<ReturnArticleDto>> UpdateArticleAsync(Guid id, UpdateArticleDto dto, ClaimsPrincipal context,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteArticleAsync(Guid id, ClaimsPrincipal context, CancellationToken cancellationToken = default);
}