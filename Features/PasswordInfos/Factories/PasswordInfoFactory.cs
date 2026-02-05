using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.PasswordInfos.PasswordInfoDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.PasswordInfos.Factories;

public class PasswordInfoFactory : BaseFactory<PasswordInfo>, IPasswordInfoFactory
{
    public PasswordInfo Create(CreatePasswordInfoDto dto)
    {
        return CreateInstance(dto);
    }

    protected override PasswordInfo CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            return new PasswordInfo
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTime.UtcNow,
                AttemptCount = 0,
                UserId = Guid.Empty
            };
        }

        return parameters switch
        {
            [CreatePasswordInfoDto dto] => new PasswordInfo
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTime.UtcNow,
                AttemptCount = dto.AttemptCount,
                UserId = dto.UserId
            },
            
            [Guid userId] => new PasswordInfo
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTime.UtcNow,
                AttemptCount = 0,
                UserId = userId
            },
            
            [Guid userId, int attemptCount] => new PasswordInfo
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTime.UtcNow,
                AttemptCount = attemptCount,
                UserId = userId
            },
            
            _ => throw new ArgumentException(
                "Paramètres invalides. Attendu : () ou (CreatePasswordInfoDto) ou (userId) ou (userId, attemptCount)")
        };
    }
}