using Ressource_API.Features.Departments.Models;
using Ressource_API.Features.Departments.DepartmentDtos;

namespace Ressource_API.Features.Departments.Services;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default);
    Task<Department?> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Department> CreateDepartmentAsync(CreateDepartmentDto dto, CancellationToken cancellationToken = default);
    Task<Department?> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteDepartmentAsync(int id, CancellationToken cancellationToken = default);
}
