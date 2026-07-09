using Ressource_API.Features.Regions.Models;
using Ressource_API.Features.Regions.RegionDtos;

namespace Ressource_API.Features.Regions.Services;

public interface IRegionService
{
    Task<IEnumerable<Region>> GetAllRegionsAsync(CancellationToken cancellationToken = default);
    Task<Region?> GetRegionByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Region> CreateRegionAsync(CreateRegionDto dto, CancellationToken cancellationToken = default);
    Task<Region?> UpdateRegionAsync(int id, UpdateRegionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRegionAsync(int id, CancellationToken cancellationToken = default);
}
