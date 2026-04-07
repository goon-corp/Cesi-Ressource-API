using Ressource_API.Features.Logins.Models;
using Ressource_API.Features.Logins.LoginDtos;
using Ressource_API.Features.Logins.Repositories;
using Ressource_API.Features.Logins.Factories;

namespace Ressource_API.Features.Logins.Services;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _repository;
    private readonly ILoginFactory _factory;

    public LoginService(
        ILoginRepository repository,
        ILoginFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Login>> GetAllLoginsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<Login?> GetLoginByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Login?> GetLoginByUserId(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetLoginByUserId(id);
    }

    public async Task<Login> CreateLoginAsync(CreateLoginDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var login = _factory.Create(dto);
        
        return await _repository.AddAsync(login, cancellationToken);
    }

    public async Task<Login?> UpdateLoginAsync(int id, UpdateLoginDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteLoginAsync(int id, CancellationToken cancellationToken = default)
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
