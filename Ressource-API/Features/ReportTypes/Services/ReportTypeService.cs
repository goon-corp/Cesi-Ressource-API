using Microsoft.Extensions.Caching.Hybrid;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.ReportTypes.Models;
using Ressource_API.Features.ReportTypes.Repositories;

namespace Ressource_API.Features.ReportTypes.Services;

public class ReportTypeService : IReportTypeService
{
    private readonly IReportTypeRepository _repository;
    private readonly HybridCache _cache;

    public ReportTypeService(IReportTypeRepository repository, HybridCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<Result<IEnumerable<ReportType>>> GetAllReportTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var reportTypes = await _cache.GetOrCreateAsync("reportTypes",
            async token => { return await _repository.ListAsync(token); });
        return reportTypes;
    }
}