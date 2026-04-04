using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Quizzes.Dtos;
using Ressource_API.Features.Quizzes.Query;

namespace Ressource_API.Features.Quizzes.Services;

public interface IQuizzService
{
    Task<Result<PaginatedList<QuizzInfoDto>>> GetPaginatedQuizzesAsync(
        QuizzQuery query,
        CancellationToken cancellationToken = default);

    Task<Result<QuizzInfoDto>> GetQuizzByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Result<QuizzInfoDto>> GetQuizzByRessourceIdAsync(
        Guid ressourceId,
        CancellationToken cancellationToken = default);

    Task<Result<QuizzInfoDto>> CreateQuizzAsync(
        CreateQuizzDto dto,
        ClaimsPrincipal context,
        CancellationToken cancellationToken = default);

    Task<Result<QuizzInfoDto>> UpdateQuizzAsync(
        Guid id,
        UpdateQuizzDto dto,
        ClaimsPrincipal context,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteQuizzAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}