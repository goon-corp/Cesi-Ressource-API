using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.QuizzQuestions.Dtos;
using Ressource_API.Features.QuizzQuestions.Query;

namespace Ressource_API.Features.QuizzQuestions.Services;

public interface IQuizzQuestionService
{
    Task<Result<PaginatedList<QuizzQuestionInfoDto>>> GetPaginatedQuizzQuestionsAsync(
        QuizzQuestionQuery query,
        CancellationToken cancellationToken = default);

    Task<Result<QuizzQuestionInfoDto>> GetQuizzQuestionByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Result<QuizzQuestionInfoDto>> CreateQuizzQuestionAsync(
        CreateQuizzQuestionDto dto,
        CancellationToken cancellationToken = default);

    Task<Result<QuizzQuestionInfoDto>> UpdateQuizzQuestionAsync(
        Guid id,
        UpdateQuizzQuestionDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteQuizzQuestionAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Result<QuizzQuestionInfoDto>> UpdateQuizzQuestionAsyncPlayer(Guid id,
        Guid userId,
        // UpdateQuizzQuestionDto dto,
        CancellationToken cancellationToken = default
    );
}