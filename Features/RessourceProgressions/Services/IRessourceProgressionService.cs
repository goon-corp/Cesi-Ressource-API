using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;

namespace Ressource_API.Features.RessourceProgressions.Services;

public interface IRessourceProgressionService
{
    Task<Result<List<RessourceProgression>>> GetAllRessourceProgressionsAsync(CancellationToken cancellationToken = default);
    
    Task<Result<RessourceProgression?>> GetRessourceProgressionByIdAsync(Guid ressourceId, Guid userId, CancellationToken cancellationToken = default);
    
    Task<Result<RessourceProgression>> CreateRessourceProgressionAsync(CreateRessourceProgressionDto dto, CancellationToken cancellationToken = default);
    
    Task<Result<RessourceProgression?>> UpdateRessourceProgressionAsync(Guid ressourceId, Guid userId, UpdateRessourceProgressionDto dto, CancellationToken cancellationToken = default);
    
    Task<Result> DeleteRessourceProgressionAsync(Guid ressourceId, Guid userId, CancellationToken cancellationToken = default);
}