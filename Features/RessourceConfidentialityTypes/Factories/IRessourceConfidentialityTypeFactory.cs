using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RessourceConfidentialityTypes.Factories;

public interface IRessourceConfidentialityTypeFactory : IBaseFactory<RessourceConfidentialityType>
{
    RessourceConfidentialityType Create(CreateRessourceConfidentialityTypeDto dto);
}
