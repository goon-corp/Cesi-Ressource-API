using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;
using Ressource_API.Features.RessourceConfidentialityTypes.Repositories;

namespace Ressource_API.Features.RessourceConfidentialityTypes.Services;

public class RessourceConfidentialityTypeService : IRessourceConfidentialityTypeService
{
    private readonly IRessourceConfidentialityTypeRepository _repository;

    public RessourceConfidentialityTypeService(IRessourceConfidentialityTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<RessourceConfidentialityTypeInfoDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var types = await _repository.ListAsync(cancellationToken);
            var dtos = types.Select(t => new RessourceConfidentialityTypeInfoDto
            {
                Id = t.Id,
                Label = t.Label
            });
            return Result.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<RessourceConfidentialityTypeInfoDto>>(ex.Message);
        }
    }
}
