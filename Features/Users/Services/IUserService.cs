using Ressource_API.Common.Pagination;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.UserDtos;

namespace Ressource_API.Features.Users.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
    Task<User?> UpdateUserAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken = default);
    Task<UserProfileDto?> GetUserProfileById(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedList<ReturnRessourceDto>> GetUserLikedRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default);

    Task<PaginatedList<ReturnRessourceDto>>
        GetUserFavoriteRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default);

    Task<PaginatedList<ReturnRessourceDto>>
        GetUserAuthoredRessourcesById(Guid id, PagedQueryParameters query, CancellationToken cancellationToken = default);
}
