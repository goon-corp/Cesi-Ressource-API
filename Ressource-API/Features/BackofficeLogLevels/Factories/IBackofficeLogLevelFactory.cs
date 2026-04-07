using Ressource_API.Features.BackofficeLogLevels.Models;
using Ressource_API.Features.BackofficeLogLevels.BackofficeLogLevelDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.BackofficeLogLevels.Factories;

public interface IBackofficeLogLevelFactory : IBaseFactory<BackofficeLogLevel>
{
    BackofficeLogLevel Create(CreateBackofficeLogLevelDto dto);
}
