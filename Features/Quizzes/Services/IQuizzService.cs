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

    Task<Result<QuizzInfoDto>> CreateQuizzAsync(
        CreateQuizzDto dto,
        CancellationToken cancellationToken = default);

    Task<Result<QuizzInfoDto>> UpdateQuizzAsync(Guid id,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteQuizzAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}