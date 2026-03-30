using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;

namespace Ressource_API.Features.RessourceConfidentialityTypes.Extensions;

public static class RessourceConfidentialityTypeExtensions
{
    extension(RessourceConfidentialityType confidentialityType)
    {
        public RessourceConfidentialityTypeInfoDto ToReturnDto()
        {
            return new()
            {
                Id = confidentialityType.Id,
                Label = confidentialityType.Label,
            };
        }
    }
}