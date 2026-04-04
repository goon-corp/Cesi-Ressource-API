using Ressource_API.Features.UserRoles.Models;
using Ressource_API.Features.UserRoles.UserRoleDtos;

namespace Ressource_API.Features.UserRoles.Extensions;

public static class UserRoleExtension
{
    extension(UserRole role)
    {
        public ReturnUserRoleDto ToReturnDto()
        {
            return new()
            {
                Id = role.Id,
                Label = role.RoleLabel
                
            };
        }
    }
}