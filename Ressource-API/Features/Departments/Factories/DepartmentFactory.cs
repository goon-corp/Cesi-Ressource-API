using Ressource_API.Features.Departments.Models;
using Ressource_API.Features.Departments.DepartmentDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Departments.Factories;

public class DepartmentFactory : BaseFactory<Department>, IDepartmentFactory
{
    /// <summary>
    /// Creates a Department from a DTO
    /// </summary>
    public Department Create(CreateDepartmentDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Department CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Department
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateDepartmentDto dto)
        {
            // Create from DTO
            return new Department
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Department creation");
    }
}
