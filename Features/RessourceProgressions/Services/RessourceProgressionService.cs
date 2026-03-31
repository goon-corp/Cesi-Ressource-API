using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;
using Ressource_API.Features.RessourceProgressions.Repositories;

namespace Ressource_API.Features.RessourceProgressions.Services;

public class RessourceProgressionService : IRessourceProgressionService
{
    private readonly IRessourceProgressionRepository _repository;

    public RessourceProgressionService(IRessourceProgressionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RessourceProgression>> GetAllRessourceProgressionsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<RessourceProgression?> GetRessourceProgressionByIdAsync(Guid ressourceId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _repository.FirstOrDefaultAsync(
            rp => rp.RessourceId == ressourceId && rp.UserId == userId,
            cancellationToken
        );
    }

    public async Task<RessourceProgression> CreateRessourceProgressionAsync(CreateRessourceProgressionDto dto, CancellationToken cancellationToken = default)
    {
        var ressourceprogression = new RessourceProgression 
        { 
            RessourceId = dto.RessourceId, 
            UserId = dto.UserId, 
            IsAside = dto.IsAside, 
            IsExploited = dto.IsExploited 
        };
        return await _repository.AddAsync(ressourceprogression, cancellationToken);
    }

    public async Task<RessourceProgression?> UpdateRessourceProgressionAsync(Guid ressourceId, Guid userId, UpdateRessourceProgressionDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await GetRessourceProgressionByIdAsync(ressourceId, userId, cancellationToken);
        
        if (existing == null) return null;

        existing.IsAside = dto.IsAside;
        existing.IsExploited = dto.IsExploited;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteRessourceProgressionAsync(Guid ressourceId, Guid userId, CancellationToken cancellationToken = default)
    {
        var existing = await GetRessourceProgressionByIdAsync(ressourceId, userId, cancellationToken);
        
        if (existing == null) return false;

        await _repository.DeleteAsync(existing, cancellationToken);
        return true;
    }
}