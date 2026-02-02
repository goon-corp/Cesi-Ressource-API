using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.RessourceDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Ressources.Factories;

public interface IRessourceFactory : IBaseFactory<Ressource>
{
    Ressource Create(CreateRessourceDto dto);
}
