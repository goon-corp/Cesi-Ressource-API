using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.UserRoles.Repositories;
using Ressource_API.Features.Users.Extensions;
using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.UserDtos;
using Ressource_API.Features.Users.Repositories;

namespace Ressource_API.Features.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IUserRoleRepository _roleRepository;

    public UserService(
        IUserRepository repository, IUserRoleRepository roleRepository)
    {
        _repository = repository;
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindWithUserRoleAsync(id);
    }
    
    
    public async Task<Result<ReturnUserDto>> UpdateUserAsync(Guid id, UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        if (existing == null)
        {
            return Result.Failure<ReturnUserDto>("User not found");
        }
        
        existing.FirstName = dto.FirstName;
        existing.LastName = dto.LastName;
        existing.UserName = dto.UserName;
        existing.IsActive = dto.IsActive;
        existing.CreationTime = dto.CreationTime;
        existing.UpdateTime = dto.UpdateTime;
        existing.DeletionTime = dto.DeletionTime;
        existing.UserRoleId = dto.UserRoleId;
        
        
        var roleExists = await _roleRepository.FindAsync(dto.UserRoleId, cancellationToken);
        if (roleExists is null)
        {
            return Result.Failure<ReturnUserDto>("Invalid user role");
        }
        existing.UserRoleId = dto.UserRoleId;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        return Result.Success<ReturnUserDto>(existing.ToReturnDto());
    }

    public async Task<Result> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return Result.Failure("User not found");
        }

        
        
        
        await _repository.DeleteAsync(existing, cancellationToken);
        
        return Result.Success("User and it's related content successfully deleted");
    }
    
    
    // PROFILE LOGIC
    
    
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

   
}
