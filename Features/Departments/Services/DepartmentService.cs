using Ressource_API.Features.Departments.Models;
using Ressource_API.Features.Departments.DepartmentDtos;
using Ressource_API.Features.Departments.Repositories;
using Ressource_API.Features.Departments.Factories;

namespace Ressource_API.Features.Departments.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;
    private readonly IDepartmentFactory _factory;

    public DepartmentService(
        IDepartmentRepository repository,
        IDepartmentFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Department?> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Department> CreateDepartmentAsync(CreateDepartmentDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var department = _factory.Create(dto);
        
        return await _repository.AddAsync(department, cancellationToken);
    }

    public async Task<Department?> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteDepartmentAsync(int id, CancellationToken cancellationToken = default)
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
