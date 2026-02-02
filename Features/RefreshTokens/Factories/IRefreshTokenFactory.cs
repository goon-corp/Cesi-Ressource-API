using Ressource_API.Features.RefreshTokens.Models;
using Ressource_API.Features.RefreshTokens.RefreshTokenDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RefreshTokens.Factories;

public interface IRefreshTokenFactory : IBaseFactory<RefreshToken>
{
    RefreshToken Create(CreateRefreshTokenDto dto);
}
