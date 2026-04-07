using Ressource_API.Features.Users.Models;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Users.Factories;

public class UserFactory : BaseFactory<User>, IUserFactory
{
    protected override User CreateInstance(params object[] parameters)
    {
        // Création par défaut avec valeurs minimales
        if (parameters.Length == 0)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = string.Empty,
                LastName = string.Empty,
                UserName = string.Empty,
                IsActive = false,
                CreationTime = DateTime.UtcNow,
                UserRoleId = Guid.Empty
            };
        }

        // Création avec paramètres typés
        return parameters switch
        {
            // Création avec userName uniquement
            [string userName] => new User
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                FirstName = string.Empty,
                LastName = string.Empty,
                IsActive = false,
                CreationTime = DateTime.UtcNow,
                UserRoleId = Guid.Empty
            },
            
            // Création avec userName, firstName, lastName
            [string userName, string firstName, string lastName] => new User
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                IsActive = false,
                CreationTime = DateTime.UtcNow,
                UserRoleId = Guid.Empty
            },
            
            // Création complète avec userName, firstName, lastName, userRoleId
            [string userName, string firstName, string lastName, Guid userRoleId] => new User
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true,
                CreationTime = DateTime.UtcNow,
                UserRoleId = userRoleId
            },
            
            _ => throw new ArgumentException(
                "Paramètres invalides. Attendu : () ou (userName) ou (userName, firstName, lastName) ou (userName, firstName, lastName, userRoleId)")
        };
    }
}