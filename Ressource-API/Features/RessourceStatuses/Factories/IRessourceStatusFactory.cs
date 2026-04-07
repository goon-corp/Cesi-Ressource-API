using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RessourceStatuses.Factories;

public interface IRessourceStatusFactory : IBaseFactory<RessourceStatus>
{
    RessourceStatus Create(CreateRessourceStatusDto dto);
}
