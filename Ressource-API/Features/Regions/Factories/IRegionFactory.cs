using Ressource_API.Features.Regions.Models;
using Ressource_API.Features.Regions.RegionDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Regions.Factories;

public interface IRegionFactory : IBaseFactory<Region>
{
    Region Create(CreateRegionDto dto);
}
