using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.UserDtos;

namespace Ressource_API.Features.Users.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<User> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
    Task<User?> UpdateUserAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken = default);
}
