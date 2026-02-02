using Ressource_API.Features.RessourceMedias.Models;
using Ressource_API.Features.RessourceMedias.RessourceMediaDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RessourceMedias.Factories;

public interface IRessourceMediaFactory : IBaseFactory<RessourceMedia>
{
    RessourceMedia Create(CreateRessourceMediaDto dto);
}
