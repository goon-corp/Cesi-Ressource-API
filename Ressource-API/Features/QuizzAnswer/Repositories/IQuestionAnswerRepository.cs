using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.QuestionAnswers.Dtos;
using Ressource_API.Features.QuizzAnswer.Models;
using Ressource_API.Features.QuizzAnswer.Query;

namespace Ressource_API.Features.QuizzAnswer.Repositories;

public interface IQuestionAnswerRepository : IBaseRepository<QuestionAnswer>
{
    Task<PaginatedList<QuestionAnswerInfoDto>> PaginatedQuestionAnswersAsync(
        QuestionAnswerQuery query,
        CancellationToken cancellationToken = default);

    Task<QuestionAnswer?> FindByUsersAndQuestionAsync(
        Guid userId,
        Guid quizzQuestionId,
        CancellationToken cancellationToken = default);
}