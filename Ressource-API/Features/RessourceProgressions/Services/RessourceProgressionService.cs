using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;
using Ressource_API.Features.RessourceProgressions.Repositories;
using Sprache;
using Result = Ressource_API.Common.ResultPattern.Result;

namespace Ressource_API.Features.RessourceProgressions.Services;

public class RessourceProgressionService : IRessourceProgressionService
{
    private readonly IRessourceProgressionRepository _repository;

    public RessourceProgressionService(IRessourceProgressionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<RessourceProgression>>> GetAllRessourceProgressionsAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.ListAsync(cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<RessourceProgression?>>GetRessourceProgressionByIdAsync(Guid ressourceId, Guid userId, CancellationToken cancellationToken = default)
    {
        var progression = await _repository.FirstOrDefaultAsync(
            rp => rp.RessourceId == ressourceId && rp.UserId == userId,
            cancellationToken
        );

        if (progression == null)
        {
            return Result.Failure<RessourceProgression?>("Progression not found.");
        }

        return Result<RessourceProgression>.Success(progression);
    }

    public async Task<Result<RessourceProgression>> CreateRessourceProgressionAsync(CreateRessourceProgressionDto dto, CancellationToken cancellationToken = default)
    {
        var ressourceprogression = new RessourceProgression 
        { 
            RessourceId = dto.RessourceId, 
            UserId = dto.UserId, 
            IsAside = dto.IsAside, 
            IsExploited = dto.IsExploited 
        };

        var created = await _repository.AddAsync(ressourceprogression, cancellationToken);
        return Result.Success(created);
    }

    public async Task<Result<RessourceProgression?>> UpdateRessourceProgressionAsync(Guid ressourceId, Guid userId, UpdateRessourceProgressionDto dto, CancellationToken cancellationToken = default)
    {
        var existingResult = await GetRessourceProgressionByIdAsync(ressourceId, userId, cancellationToken);
        
        if (!existingResult.IsSuccess) 
        {
            return Result.Failure<RessourceProgression?>("Progression not found.");
        }

        var entity = existingResult.Data;
        entity.IsAside = dto.IsAside;
        entity.IsExploited = dto.IsExploited;
        
        await _repository.UpdateAsync(entity, cancellationToken);
        return Result.Success(entity);
    }

    public async Task<Result> DeleteRessourceProgressionAsync(Guid ressourceId, Guid userId, CancellationToken cancellationToken = default)
    {
        var existingResult = await GetRessourceProgressionByIdAsync(ressourceId, userId, cancellationToken);
        
        if (!existingResult.IsSuccess)
        {
            return Result.Failure(existingResult.Error);
        }

        await _repository.DeleteAsync(existingResult.Data, cancellationToken);
        return Result.Success();
    }
}