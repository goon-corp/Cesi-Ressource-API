using Ressource_API.Features.Addresses.Models;
using Ressource_API.Features.Addresses.AddressDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Addresses.Factories;

public interface IAddressFactory : IBaseFactory<Address>
{
    Address Create(CreateAddressDto dto);
}
