using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.PasswordInfos.PasswordInfoDtos;
using Ressource_API.Features.PasswordInfos.Repositories;
using Ressource_API.Features.PasswordInfos.Factories;

namespace Ressource_API.Features.PasswordInfos.Services;

public class PasswordInfoService : IPasswordInfoService
{
    private readonly IPasswordInfoRepository _repository;
    private readonly IPasswordInfoFactory _factory;

    public PasswordInfoService(
        IPasswordInfoRepository repository,
        IPasswordInfoFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<PasswordInfo>> GetAllPasswordInfosAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<PasswordInfo?> GetPasswordInfoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<PasswordInfo> CreatePasswordInfoAsync(CreatePasswordInfoDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var passwordinfo = _factory.Create(dto);
        
        return await _repository.AddAsync(passwordinfo, cancellationToken);
    }

    public async Task<PasswordInfo?> UpdatePasswordInfoAsync(int id, UpdatePasswordInfoDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeletePasswordInfoAsync(int id, CancellationToken cancellationToken = default)
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
