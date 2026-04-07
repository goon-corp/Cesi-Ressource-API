using Ressource_API.Common.Data;
using Ressource_API.Features.EmailLogs.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.EmailLogs.Repositories;

public class EmailLogRepository : BaseRepository<EmailLog>, IEmailLogRepository
{
    public EmailLogRepository(ApplicationDbContext context) : base(context)
    {
    }
}
