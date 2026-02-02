using Ressource_API.Features.UserRoles.Models;
using Ressource_API.Features.UserRoles.UserRoleDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.UserRoles.Factories;

public interface IUserRoleFactory : IBaseFactory<UserRole>
{
    UserRole Create(CreateUserRoleDto dto);
}
