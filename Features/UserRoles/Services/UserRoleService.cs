using Ressource_API.Features.UserRoles.Models;
using Ressource_API.Features.UserRoles.UserRoleDtos;
using Ressource_API.Features.UserRoles.Repositories;
using Ressource_API.Features.UserRoles.Factories;

namespace Ressource_API.Features.UserRoles.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _repository;
    private readonly IUserRoleFactory _factory;

    public UserRoleService(
        IUserRoleRepository repository,
        IUserRoleFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<UserRole?> GetUserRoleByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<UserRole> CreateUserRoleAsync(CreateUserRoleDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var userrole = _factory.Create(dto);
        
        return await _repository.AddAsync(userrole, cancellationToken);
    }

    public async Task<UserRole?> UpdateUserRoleAsync(int id, UpdateUserRoleDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteUserRoleAsync(int id, CancellationToken cancellationToken = default)
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
