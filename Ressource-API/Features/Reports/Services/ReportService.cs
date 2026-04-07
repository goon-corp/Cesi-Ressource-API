using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Reports.Dtos;
using Ressource_API.Features.Reports.Extensions;
using Ressource_API.Features.Reports.Factories;
using Ressource_API.Features.Reports.Query;
using Ressource_API.Features.Reports.Repositories;

namespace Ressource_API.Features.Reports.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _repository;
    private readonly IReportFactory _factory;

    public ReportService(IReportRepository repository, IReportFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<Result<PaginatedList<ReportInfoDto>>> GetPaginatedReportsAsync(
        ReportQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.PaginatedReportsAsync(query, cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<ReportInfoDto>> GetReportByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var report = await _repository.FindByIdAsync(id, cancellationToken);

        if (report == null)
            return Result.Failure<ReportInfoDto>("Report not found");

        return Result.Success(report.ToInfoDto());
    }

    public async Task<Result<ReportInfoDto>> CreateReportAsync(
        CreateReportDto dto,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        var currentUserIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserIdStr) || !Guid.TryParse(currentUserIdStr, out var userId))
            return Result.Failure<ReportInfoDto>("User not authenticated or invalid user ID");

        var userAlreadyReported = await _repository.FirstOrDefaultAsyncAsNoTracking(r => r.UserId == userId && r.RessourceId == dto.RessourceId , cancellationToken);
        
        if(userAlreadyReported is not null) return Result.Failure<ReportInfoDto>("You already reported that.");
        
        var report = _factory.Create(dto, userId);
        var created = await _repository.AddAsync(report, cancellationToken);

        return Result.Success(created.ToInfoDto());
    }

    public async Task<Result<ReportInfoDto>> UpdateReportAsync(
        Guid id,
        UpdateReportDto dto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure<ReportInfoDto>("Report not found");

        existing.IsCheckedByModerator = dto.IsCheckedByModerator;
        existing.UpdateTime = DateTime.UtcNow;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(existing.ToInfoDto());
    }

    public async Task<Result> DeleteReportAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(id, cancellationToken);

        if (existing == null)
            return Result.Failure("Report not found");

        await _repository.DeleteAsync(existing, cancellationToken);

        return Result.Success();
    }
}