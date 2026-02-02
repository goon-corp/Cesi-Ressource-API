using Ressource_API.Features.UserRoles.Models;
using Ressource_API.Features.UserRoles.UserRoleDtos;

namespace Ressource_API.Features.UserRoles.Services;

public interface IUserRoleService
{
    Task<IEnumerable<UserRole>> GetAllUserRolesAsync(CancellationToken cancellationToken = default);
    Task<UserRole?> GetUserRoleByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserRole> CreateUserRoleAsync(CreateUserRoleDto dto, CancellationToken cancellationToken = default);
    Task<UserRole?> UpdateUserRoleAsync(int id, UpdateUserRoleDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserRoleAsync(int id, CancellationToken cancellationToken = default);
}
