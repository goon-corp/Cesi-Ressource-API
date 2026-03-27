using Ressource_API.Features.Reports.Models;
using Ressource_API.Features.Reports.ReportDtos;
using Ressource_API.Features.Reports.Repositories;
using Ressource_API.Features.Reports.Factories;

namespace Ressource_API.Features.Reports.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _repository;
    private readonly IReportFactory _factory;

    public ReportService(
        IReportRepository repository,
        IReportFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Report>> GetAllReportsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Report?> GetReportByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Report> CreateReportAsync(CreateReportDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var report = _factory.Create(dto);
        
        return await _repository.AddAsync(report, cancellationToken);
    }

    public async Task<Report?> UpdateReportAsync(int id, UpdateReportDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return null;
        }

        // TODO: Map properties from dto to existing
        // Example: existing.Name = dto.Name;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        
        return existing;
    }

    public async Task<bool> DeleteReportAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);
        
        return true;
    }
}
