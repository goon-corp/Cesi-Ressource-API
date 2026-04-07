using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.RessourceTypes.Repositories;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;

namespace Ressource_API.Features.RessourceTypes.Services;

public class RessourceTypeService : IRessourceTypeService
{
    private readonly IRessourceTypeRepository _repository;

    public RessourceTypeService(
        IRessourceTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ReturnRessourceTypeDTO>> GetAllRessourceTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAllAsync(cancellationToken);
    }
}
