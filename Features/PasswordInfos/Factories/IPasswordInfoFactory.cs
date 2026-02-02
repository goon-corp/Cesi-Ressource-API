using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.PasswordInfos.PasswordInfoDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.PasswordInfos.Factories;

public interface IPasswordInfoFactory : IBaseFactory<PasswordInfo>
{
    PasswordInfo Create(CreatePasswordInfoDto dto);
}
