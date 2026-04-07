using Ressource_API.Common.Data;
using Ressource_API.Features.Departments.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Departments.Repositories;

public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext context) : base(context)
    {
    }
}
