using Ressource_API.Features.Cities.Models;
using Ressource_API.Features.Cities.CityDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Cities.Factories;

public interface ICityFactory : IBaseFactory<City>
{
    City Create(CreateCityDto dto);
}
