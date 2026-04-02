using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.QuestionAnswers.Dtos;
using Ressource_API.Features.QuizzAnswer.Query;

namespace Ressource_API.Features.QuizzAnswer.Services;

public interface IQuestionAnswerService
{
    Task<Result<PaginatedList<QuestionAnswerInfoDto>>> GetPaginatedQuestionAnswersAsync(
        QuestionAnswerQuery query,
        CancellationToken cancellationToken = default);

    Task<Result<QuestionAnswerInfoDto>> GetQuestionAnswerAsync(
        Guid userId,
        Guid quizzQuestionId,
        CancellationToken cancellationToken = default);

    Task<Result<QuestionAnswerInfoDto>> CreateQuestionAnswerAsync(
        CreateQuestionAnswerDto dto,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default);

    Task<Result<QuestionAnswerInfoDto>> UpdateQuestionAnswerAsync(
        Guid userId,
        Guid quizzQuestionId,
        UpdateQuestionAnswerDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteQuestionAnswerAsync(
        Guid userId,
        Guid quizzQuestionId,
        CancellationToken cancellationToken = default);
}