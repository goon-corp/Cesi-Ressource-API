using Ressource_API.Common.Data;
using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Common.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;

namespace Ressource_API.Features.RessourceTypes.Repositories;

public class RessourceTypeRepository : BaseRepository<RessourceType>, IRessourceTypeRepository
{
    public RessourceTypeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ReturnRessourceTypeDTO>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<RessourceType>()
            .Select(rt => new ReturnRessourceTypeDTO
            {
                Id = rt.Id,
                Label = rt.Label 
            })
            .ToListAsync(cancellationToken);
    }
}