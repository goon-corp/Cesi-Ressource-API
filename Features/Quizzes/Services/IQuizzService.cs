using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.Quizzes.QuizzDtos;

namespace Ressource_API.Features.Quizzes.Services;

public interface IQuizzeservice
{
    Task<IEnumerable<Quizz>> GetAllQuizzesAsync(CancellationToken cancellationToken = default);
    Task<Quizz?> GetQuizzByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Quizz> CreateQuizzAsync(CreateQuizzDto dto, CancellationToken cancellationToken = default);
    Task<Quizz?> UpdateQuizzAsync(int id, UpdateQuizzDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteQuizzAsync(int id, CancellationToken cancellationToken = default);
}
