using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.PollOptions.PollOptionDtos;
using Ressource_API.Features.PollOptions.Repositories;
using Ressource_API.Features.PollOptions.Factories;

namespace Ressource_API.Features.PollOptions.Services;

public class PollOptionService : IPollOptionService
{
    private readonly IPollOptionRepository _repository;
    private readonly IPollOptionFactory _factory;

    public PollOptionService(
        IPollOptionRepository repository,
        IPollOptionFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<PollOption>> GetAllPollOptionsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<PollOption?> GetPollOptionByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<PollOption> CreatePollOptionAsync(CreatePollOptionDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var polloption = _factory.Create(dto);
        
        return await _repository.AddAsync(polloption, cancellationToken);
    }

    public async Task<PollOption?> UpdatePollOptionAsync(int id, UpdatePollOptionDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeletePollOptionAsync(int id, CancellationToken cancellationToken = default)
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
