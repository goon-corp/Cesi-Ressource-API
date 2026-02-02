using Ressource_API.Features.Cities.Models;
using Ressource_API.Features.Cities.CityDtos;

namespace Ressource_API.Features.Cities.Services;

public interface ICitieservice
{
    Task<IEnumerable<City>> GetAllCitiesAsync(CancellationToken cancellationToken = default);
    Task<City?> GetCityByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<City> CreateCityAsync(CreateCityDto dto, CancellationToken cancellationToken = default);
    Task<City?> UpdateCityAsync(int id, UpdateCityDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteCityAsync(int id, CancellationToken cancellationToken = default);
}
