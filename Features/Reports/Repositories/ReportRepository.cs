using Ressource_API.Common.Data;
using Ressource_API.Features.Reports.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Reports.Repositories;

public class ReportRepository : BaseRepository<Report>, IReportRepository
{
    public ReportRepository(ApplicationDbContext context) : base(context)
    {
    }
}
