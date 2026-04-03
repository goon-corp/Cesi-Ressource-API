using Ressource_API.Common.Pagination;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.UserDtos;
using Ressource_API.Features.Users.Repositories;
using Ressource_API.Features.Users.Factories;

namespace Ressource_API.Features.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IUserFactory _factory;

    public UserService(
        IUserRepository repository,
        IUserFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindWithUserRoleAsync(id);
    }
    
    public async Task<UserProfileDto?> GetUserProfileById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetUserProfileAsync(id);
    }
    
    public async Task<PaginatedList<ReturnRessourceDto>> GetUserLikedRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default)
    {
        return await _repository.GetUserLikedRessourcesAsync(id, query);
    }
    
    public async Task<PaginatedList<ReturnRessourceDto>> GetUserFavoriteRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default)
    {
        return await _repository.GetUserFavoriteRessourcesAsync(id, query);
    }
    
    public async Task<PaginatedList<ReturnRessourceDto>> GetUserAuthoredRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default)
    {
        return await _repository.GetUserAuthoredRessourcesAsync(id,query);
    }

    public async Task<User> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var user = _factory.Create(dto);
        
        return await _repository.AddAsync(user, cancellationToken);
    }

    public async Task<User?> UpdateUserAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken = default)
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
