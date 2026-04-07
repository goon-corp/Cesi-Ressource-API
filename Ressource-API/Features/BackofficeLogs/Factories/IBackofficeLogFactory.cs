using Ressource_API.Features.BackofficeLogs.Models;
using Ressource_API.Features.BackofficeLogs.BackofficeLogDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.BackofficeLogs.Factories;

public interface IBackofficeLogFactory : IBaseFactory<BackofficeLog>
{
    BackofficeLog Create(CreateBackofficeLogDto dto);
}
