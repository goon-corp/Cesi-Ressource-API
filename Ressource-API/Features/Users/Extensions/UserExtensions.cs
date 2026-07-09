using Ressource_API.Features.UserRoles.Extensions;
using Ressource_API.Features.UserRoles.UserRoleDtos;
using Ressource_API.Features.Users.Dtos;
using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.UserDtos;

namespace Ressource_API.Features.Users.Extensions;

public static class UserExtensions
{
    extension(User user)
    {
        public ReturnUserDto ToReturnDto()
        {
            return new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                IsActive = user.IsActive,
                CreationTime = user.CreationTime,
                UpdateTime = user.UpdateTime,
                DeletionTime = user.DeletionTime,
                UserRole = user.UserRole.ToReturnDto()
                
            };
        }
        
        public User ToUser()
        {
            return new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                IsActive = user.IsActive,
                CreationTime = user.CreationTime,
                UpdateTime = user.UpdateTime,
                DeletionTime = user.DeletionTime,
                UserRoleId = user.UserRoleId
            };
        }
        
        public UserInfoDto ToInfoDto()
        {
            return new UserInfoDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                IsActive = user.IsActive,
                UserRoleId = user.UserRoleId,
                CreationTime = user.CreationTime,
                UpdateTime = user.UpdateTime,
                DeletionTime = user.DeletionTime
            };
        }
    }
}