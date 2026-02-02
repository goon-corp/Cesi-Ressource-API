using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.QuizzQuestions.QuizzQuestionDtos;

namespace Ressource_API.Features.QuizzQuestions.Services;

public interface IQuizzQuestionService
{
    Task<IEnumerable<QuizzQuestion>> GetAllQuizzQuestionsAsync(CancellationToken cancellationToken = default);
    Task<QuizzQuestion?> GetQuizzQuestionByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<QuizzQuestion> CreateQuizzQuestionAsync(CreateQuizzQuestionDto dto, CancellationToken cancellationToken = default);
    Task<QuizzQuestion?> UpdateQuizzQuestionAsync(int id, UpdateQuizzQuestionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteQuizzQuestionAsync(int id, CancellationToken cancellationToken = default);
}
