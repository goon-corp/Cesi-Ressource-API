using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.QuizzQuestions.Dtos;
using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.QuizzQuestions.Query;

namespace Ressource_API.Features.QuizzQuestions.Repositories;

public interface IQuizzQuestionRepository : IBaseRepository<QuizzQuestion>
{
    Task<PaginatedList<QuizzQuestionInfoDto>> PaginatedQuizzQuestionsAsync(
        QuizzQuestionQuery query,
        CancellationToken cancellationToken = default);

    Task<QuizzQuestion?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}