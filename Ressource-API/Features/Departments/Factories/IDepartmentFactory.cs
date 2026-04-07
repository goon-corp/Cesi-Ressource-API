using Ressource_API.Features.Departments.Models;
using Ressource_API.Features.Departments.DepartmentDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Departments.Factories;

public interface IDepartmentFactory : IBaseFactory<Department>
{
    Department Create(CreateDepartmentDto dto);
}
