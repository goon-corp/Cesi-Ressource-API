using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;
using Ressource_API.Features.RessourceStatuses.Repositories;

namespace Ressource_API.Features.RessourceStatuses.Services;

public class RessourceStatusService : IRessourceStatusService
{
    private readonly IRessourceStatusRepository _repository;

    public RessourceStatusService(IRessourceStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<RessourceStatusInfoDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var statuses = await _repository.ListAsync(cancellationToken);
            var dtos = statuses.Select(s => new RessourceStatusInfoDto
            {
                Id = s.Id,
                Label = s.Label
            });
            return Result.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<RessourceStatusInfoDto>>(ex.Message);
        }
    }
}
