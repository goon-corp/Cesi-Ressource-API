using Ressource_API.Features.Users.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Users.UserDtos;

namespace Ressource_API.Features.Users.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindWithUserRoleAsync(Guid userId);
    Task<UserProfileDto> GetUserProfileAsync(Guid userId);
    Task<PaginatedList<ReturnRessourceDto>> GetUserAuthoredRessourcesAsync(Guid userId, PagedQueryParameters query, CancellationToken cancellationToken = default);
    Task<PaginatedList<ReturnRessourceDto>> GetUserLikedRessourcesAsync(Guid userId, PagedQueryParameters query, CancellationToken cancellationToken = default);
    Task<PaginatedList<ReturnRessourceDto>> GetUserFavoriteRessourcesAsync(Guid userId, PagedQueryParameters query, CancellationToken cancellationToken = default);
}
