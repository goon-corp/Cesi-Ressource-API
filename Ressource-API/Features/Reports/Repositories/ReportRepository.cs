using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Reports.Dtos;
using Ressource_API.Features.Reports.Extensions;
using Ressource_API.Features.Reports.Models;
using Ressource_API.Features.Reports.Query;

namespace Ressource_API.Features.Reports.Repositories;

public class ReportRepository : BaseRepository<Report>, IReportRepository
{
    public ReportRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PaginatedList<ReportInfoDto>> PaginatedReportsAsync(
        ReportQuery query,
        CancellationToken cancellationToken = default)
    {
        var reports = _context.Reports
            .AsQueryable()
            .OrderByDescending(r => r.CreationTime);

        if (query.UserId.HasValue)
            reports = reports.Where(r => r.UserId == query.UserId.Value) as IOrderedQueryable<Report>;

        if (query.RessourceId.HasValue)
            if (reports != null)
                reports = reports.Where(r => r.RessourceId == query.RessourceId.Value) as IOrderedQueryable<Report>;

        if (query.ReportTypeId.HasValue)
            if (reports != null)
                reports = reports.Where(r => r.ReportTypeId == query.ReportTypeId.Value) as IOrderedQueryable<Report>;

        if (query.IsCheckedByModerator.HasValue)
            if (reports != null)
                reports = reports.Where(r => r.IsCheckedByModerator == query.IsCheckedByModerator.Value) as IOrderedQueryable<Report>;

        if (query.CreatedAt.HasValue)
        {
            var date = query.CreatedAt.Value.ToDateTime(TimeOnly.MinValue);
            if (reports != null) 
                reports = reports.Where(r => r.CreationTime.Date == date) as IOrderedQueryable<Report>;
        }

        var totalCount = await reports.CountAsync(cancellationToken);

        var entities = await reports
            .Include(r => r.ReportType)
            .Include(r => r.Ressource)
            .Include(r => r.User)
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ReportInfoDto>(
            entities.Select(r => r.ToInfoDto()).ToList(),
            query.page, query.size, totalCount);
    }

    public async Task<Report?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Reports
            .Include(r => r.ReportType)
            .Include(r => r.Ressource)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}