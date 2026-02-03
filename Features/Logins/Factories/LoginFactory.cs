using Ressource_API.Features.Logins.Models;
using Ressource_API.Features.Logins.LoginDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Logins.Factories;

public class LoginFactory : BaseFactory<Login>, ILoginFactory
{
    /// <summary>
    /// Creates a Login from a DTO
    /// </summary>
    public Login Create(CreateLoginDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Login CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            return new Login
            {
                Id = Guid.NewGuid(),
                Email = string.Empty,
                PasswordHash = string.Empty,
                CreationTime = DateTime.UtcNow,
                UserId = Guid.Empty
            };
        }

        return parameters switch
        {
            [CreateLoginDto dto] => new Login
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                CreationTime = DateTime.UtcNow,
                UserId = dto.UserId
            },
            
            [string email] => new Login
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = string.Empty,
                CreationTime = DateTime.UtcNow,
                UserId = Guid.Empty
            },
            
            [string email, string passwordHash, string passwordSalt] => new Login
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = passwordHash,
                CreationTime = DateTime.UtcNow,
                UserId = Guid.Empty
            },
            
            [string email, string passwordHash, string passwordSalt, Guid userId] => new Login
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = passwordHash,
                CreationTime = DateTime.UtcNow,
                UserId = userId
            },
            
            _ => throw new ArgumentException(
                "Paramètres invalides. Attendu : () ou (CreateLoginDto) ou (email) ou (email, passwordHash, passwordSalt) ou (email, passwordHash, passwordSalt, userId)")
        };
    }
}