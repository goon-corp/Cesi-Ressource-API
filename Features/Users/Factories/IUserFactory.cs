using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.UserDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Users.Factories;

public interface IUserFactory : IBaseFactory<User>
{
    User Create(CreateUserDto dto);
}
