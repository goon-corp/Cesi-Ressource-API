using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;

namespace Ressource_API.Features.RessourceTypes.Extensions;

public static class RessourceTypesExtensions
{
    extension(RessourceType type)
    {
        public RessourceTypeInfoDto ToReturnDto()
        {
            return new()
            {
                Id = type.Id,
                Label = type.Label,
            };
        }
    }
}