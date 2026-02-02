using Ressource_API.Features.ReportTypes.Models;
using Ressource_API.Features.ReportTypes.ReportTypeDtos;
using Ressource_API.Features.ReportTypes.Repositories;
using Ressource_API.Features.ReportTypes.Factories;

namespace Ressource_API.Features.ReportTypes.Services;

public class ReportTypeService : IReportTypeService
{
    private readonly IReportTypeRepository _repository;
    private readonly IReportTypeFactory _factory;

    public ReportTypeService(
        IReportTypeRepository repository,
        IReportTypeFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<ReportType>> GetAllReportTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<ReportType?> GetReportTypeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<ReportType> CreateReportTypeAsync(CreateReportTypeDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var reporttype = _factory.Create(dto);
        
        return await _repository.AddAsync(reporttype, cancellationToken);
    }

    public async Task<ReportType?> UpdateReportTypeAsync(int id, UpdateReportTypeDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteReportTypeAsync(int id, CancellationToken cancellationToken = default)
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
