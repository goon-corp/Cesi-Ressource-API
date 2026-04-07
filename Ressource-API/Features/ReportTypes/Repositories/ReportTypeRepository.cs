using Ressource_API.Common.Data;
using Ressource_API.Features.ReportTypes.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.ReportTypes.Repositories;

public class ReportTypeRepository : BaseRepository<ReportType>, IReportTypeRepository
{
    public ReportTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
