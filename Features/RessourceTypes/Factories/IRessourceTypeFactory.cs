using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RessourceTypes.Factories;

public interface IRessourceTypeFactory : IBaseFactory<RessourceType>
{
    RessourceType Create(CreateRessourceTypeDto dto);
}
