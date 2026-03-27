using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.QuizzQuestions.QuizzQuestionDtos;
using Ressource_API.Features.QuizzQuestions.Repositories;
using Ressource_API.Features.QuizzQuestions.Factories;

namespace Ressource_API.Features.QuizzQuestions.Services;

public class QuizzQuestionService : IQuizzQuestionService
{
    private readonly IQuizzQuestionRepository _repository;
    private readonly IQuizzQuestionFactory _factory;

    public QuizzQuestionService(
        IQuizzQuestionRepository repository,
        IQuizzQuestionFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<QuizzQuestion>> GetAllQuizzQuestionsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<QuizzQuestion?> GetQuizzQuestionByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<QuizzQuestion> CreateQuizzQuestionAsync(CreateQuizzQuestionDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var quizzquestion = _factory.Create(dto);
        
        return await _repository.AddAsync(quizzquestion, cancellationToken);
    }

    public async Task<QuizzQuestion?> UpdateQuizzQuestionAsync(int id, UpdateQuizzQuestionDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return null;
        }

        // TODO: Map properties from dto to existing
        // Example: existing.Name = dto.Name;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        
        return existing;
    }

    public async Task<bool> DeleteQuizzQuestionAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);
        
        return true;
    }
}
