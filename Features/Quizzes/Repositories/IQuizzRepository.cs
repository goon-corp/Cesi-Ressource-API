using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Quizzes.Dtos;
using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.Quizzes.Query;

namespace Ressource_API.Features.Quizzes.Repositories;

public interface IQuizzRepository : IBaseRepository<Quizz>
{
    Task<PaginatedList<QuizzInfoDto>> PaginatedQuizzesAsync(
        QuizzQuery query,
        CancellationToken cancellationToken = default);

    Task<Quizz?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Quizz?> GetQuizzNoTrackingByRessourceId(
        Guid ressourceId,
        CancellationToken cancellationToken = default);
}