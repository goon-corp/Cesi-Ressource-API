using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;

namespace Ressource_API.Features.RessourceStatuses.Services;

public interface IRessourceStatusService
{
    Task<Result<IEnumerable<RessourceStatusInfoDto>>> GetAllAsync(CancellationToken cancellationToken = default);
}
