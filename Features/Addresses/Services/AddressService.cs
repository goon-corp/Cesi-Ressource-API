using Ressource_API.Features.Addresses.Models;
using Ressource_API.Features.Addresses.AddressDtos;
using Ressource_API.Features.Addresses.Repositories;
using Ressource_API.Features.Addresses.Factories;

namespace Ressource_API.Features.Addresses.Services;

public class Addresseservice : IAddresseservice
{
    private readonly IAddressRepository _repository;
    private readonly IAddressFactory _factory;

    public Addresseservice(
        IAddressRepository repository,
        IAddressFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Address>> GetAllAddressesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Address?> GetAddressByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Address> CreateAddressAsync(CreateAddressDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var address = _factory.Create(dto);
        
        return await _repository.AddAsync(address, cancellationToken);
    }

    public async Task<Address?> UpdateAddressAsync(int id, UpdateAddressDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return null;
        }

        // TODO: Map properties from dto to existing
        // Example: existing.Name = dto.Name;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        
        return existing;
    }

    public async Task<bool> DeleteAddressAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);
        
        return true;
    }
}
