using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.Quizzes.QuizzDtos;
using Ressource_API.Features.Quizzes.Repositories;
using Ressource_API.Features.Quizzes.Factories;

namespace Ressource_API.Features.Quizzes.Services;

public class Quizzeservice : IQuizzeservice
{
    private readonly IQuizzRepository _repository;
    private readonly IQuizzFactory _factory;

    public Quizzeservice(
        IQuizzRepository repository,
        IQuizzFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Quizz>> GetAllQuizzesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<Quizz?> GetQuizzByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Quizz> CreateQuizzAsync(CreateQuizzDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var quizz = _factory.Create(dto);
        
        return await _repository.AddAsync(quizz, cancellationToken);
    }

    public async Task<Quizz?> UpdateQuizzAsync(int id, UpdateQuizzDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteQuizzAsync(int id, CancellationToken cancellationToken = default)
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
