using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Features.PasswordHistories.PasswordHistoryDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.PasswordHistories.Factories;

public interface IPasswordHistoryFactory : IBaseFactory<PasswordHistory>
{
    PasswordHistory Create(CreatePasswordHistoryDto dto);
}
