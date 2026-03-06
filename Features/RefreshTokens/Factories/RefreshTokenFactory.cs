using Ressource_API.Features.RefreshTokens.Models;
using Ressource_API.Common.Data.Factories;
using Ressource_API.Features.RefreshTokens.RefreshTokenDtos;

namespace Ressource_API.Features.RefreshTokens.Factories;

public class RefreshTokenFactory : BaseFactory<RefreshToken>, IRefreshTokenFactory
{
    public RefreshToken Create(CreateRefreshTokenDto dto)
    {
        return CreateInstance(dto);
    }
    
    protected override RefreshToken CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = string.Empty,
                IsActive = true,
                ExpirationTime = DateTime.UtcNow.AddDays(7),
                CreationTime = DateTime.UtcNow,
                UserId = Guid.Empty
            };
        }

        return parameters switch
        {
            [Guid userId, string token, DateTime expiresAt] => new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = token,
                IsActive = true,
                ExpirationTime = expiresAt,
                UserId = userId,
                CreationTime = DateTime.UtcNow
            },

            [Guid userId, string token, TimeSpan validity] => new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = token,
                IsActive = true,
                ExpirationTime = DateTime.UtcNow.Add(validity),
                UserId = userId,
                CreationTime = DateTime.UtcNow
            },

            _ => throw new ArgumentException(
                "Paramètres invalides. Attendu : () ou (userId, token, expiresAt) ou (userId, token, validity)")
        };
    }
}