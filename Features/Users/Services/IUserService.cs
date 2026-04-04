using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Users.Dtos;
using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.Query;
using Ressource_API.Features.Users.UserDtos;

namespace Ressource_API.Features.Users.Services;

public interface IUserService
{
    Task<Result<PaginatedList<UserInfoDto>>> GetPaginatedUsersAsync(
        UserQuery query,
        CancellationToken cancellationToken = default);

    Task<Result<UserInfoDto>> GetUserByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Result<UserInfoDto>> GetCurrentUserAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default);

    Task<Result<UserInfoDto>> CreateUserAsync(
        CreateUserDto dto,
        CancellationToken cancellationToken = default);

    Task<Result<UserInfoDto>> UpdateUserAsync(
        Guid id,
        UpdateUserDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteUserAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    Task<UserProfileDto?> GetUserProfileById(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedList<ReturnRessourceDto>> GetUserLikedRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default);

    Task<PaginatedList<ReturnRessourceDto>>
        GetUserFavoriteRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default);

    Task<PaginatedList<ReturnRessourceDto>>
        GetUserAuthoredRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default);
}
