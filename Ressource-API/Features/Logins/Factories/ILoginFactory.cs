using Ressource_API.Features.Logins.Models;
using Ressource_API.Features.Logins.LoginDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Logins.Factories;

public interface ILoginFactory : IBaseFactory<Login>
{
    Login Create(CreateLoginDto dto);
}
