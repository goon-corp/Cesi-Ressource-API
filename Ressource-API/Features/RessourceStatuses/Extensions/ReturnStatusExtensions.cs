using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;

namespace Ressource_API.Features.RessourceStatuses.Extensions;

public static class ReturnStatusExtensions
{
    extension(RessourceStatus status)
    {
        public RessourceStatusInfoDto ToReturnDto()
        {
            return new()
            {
                Id = status.Id,
                Label = status.Label,
            };
        }
    }
}