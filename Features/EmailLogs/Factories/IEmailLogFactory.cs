using Ressource_API.Features.EmailLogs.Models;
using Ressource_API.Features.EmailLogs.EmailLogDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.EmailLogs.Factories;

public interface IEmailLogFactory : IBaseFactory<EmailLog>
{
    EmailLog Create(CreateEmailLogDto dto);
}
