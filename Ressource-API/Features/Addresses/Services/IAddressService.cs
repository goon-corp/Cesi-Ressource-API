using Ressource_API.Features.Addresses.Models;
using Ressource_API.Features.Addresses.AddressDtos;

namespace Ressource_API.Features.Addresses.Services;

public interface IAddresseservice
{
    Task<IEnumerable<Address>> GetAllAddressesAsync(CancellationToken cancellationToken = default);
    Task<Address?> GetAddressByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Address> CreateAddressAsync(CreateAddressDto dto, CancellationToken cancellationToken = default);
    Task<Address?> UpdateAddressAsync(int id, UpdateAddressDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAddressAsync(int id, CancellationToken cancellationToken = default);
}
