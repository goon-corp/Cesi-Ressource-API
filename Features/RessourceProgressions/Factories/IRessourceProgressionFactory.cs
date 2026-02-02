using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RessourceProgressions.Factories;

public interface IRessourceProgressionFactory : IBaseFactory<RessourceProgression>
{
    RessourceProgression Create(CreateRessourceProgressionDto dto);
}
