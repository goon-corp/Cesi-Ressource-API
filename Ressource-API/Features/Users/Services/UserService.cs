using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.UserRoles.Repositories;
using Ressource_API.Features.Users.Dtos;
using Ressource_API.Features.Users.Extensions;
using Ressource_API.Features.Users.Factories;
using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.Query;
using Ressource_API.Features.Users.UserDtos;
using Ressource_API.Features.Users.Repositories;

namespace Ressource_API.Features.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IUserRoleRepository _roleRepository;
    private readonly IUserFactory _factory;

    public UserService(
        IUserRepository repository, IUserRoleRepository roleRepository, IUserFactory factory)
    {
        _repository = repository;
        _roleRepository = roleRepository;
        _factory = factory;
    }

public async Task<Result<PaginatedList<UserInfoDto>>> GetPaginatedUsersAsync(
        UserQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.PaginatedUsersAsync(query, cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<UserInfoDto>> GetUserByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await _repository.FindByIdAsync(id, cancellationToken);

        if (user == null)
            return Result.Failure<UserInfoDto>("User not found");

        return Result.Success(user.ToInfoDto());
    }

    public async Task<Result<UserInfoDto>> GetCurrentUserAsync(
        ClaimsPrincipal principal,
        CancellationToken cancellationToken = default)
    {
        var currentUserIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserIdStr) || !Guid.TryParse(currentUserIdStr, out var userId))
            return Result.Failure<UserInfoDto>("User not authenticated or invalid user ID");

        var user = await _repository.FindByIdAsync(userId, cancellationToken);

        if (user == null)
            return Result.Failure<UserInfoDto>("User not found");

        return Result.Success(user.ToInfoDto());
    }

    public async Task<Result<UserInfoDto>> CreateUserAsync(
        CreateUserDto dto,
        CancellationToken cancellationToken = default)
    {
        var alreadyExists = await _repository.FindByUserNameAsync(dto.UserName, cancellationToken);

        if (alreadyExists != null)
            return Result.Failure<UserInfoDto>("Username is already taken");

        var user = _factory.Create(dto);
        var created = await _repository.AddAsync(user, cancellationToken);

        return Result.Success(created.ToInfoDto());
    }

    public async Task<Result<UserInfoDto>> UpdateUserAsync(
        Guid id,
        UpdateUserDto dto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure<UserInfoDto>("User not found");

        if (existing.UserName != dto.UserName)
        {
            var userNameTaken = await _repository.FindByUserNameAsync(dto.UserName, cancellationToken);
            if (userNameTaken != null)
                return Result.Failure<UserInfoDto>("Username is already taken");
        }

        existing.FirstName = dto.FirstName;
        existing.LastName = dto.LastName;
        existing.UserName = dto.UserName;
        existing.UserRoleId = dto.UserRoleId;
        existing.UpdateTime = DateTime.UtcNow;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(existing.ToInfoDto());
    }

    public async Task<Result> DeleteUserAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure("User not found");

        existing.DeletionTime = DateTime.UtcNow;
        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success();
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
        return await _repository.GetUserAuthoredRessourcesAsync(id, query);
    }

    public async Task<PaginatedList<ReturnRessourceDto>> GetUserAsideRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default)
    {
        return await _repository.GetUserAsideRessourcesAsync(id, query, cancellationToken);
    }

    public async Task<PaginatedList<ReturnRessourceDto>> GetUserExploitedRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default)
    {
        return await _repository.GetUserExploitedRessourcesAsync(id, query, cancellationToken);
    }
}
