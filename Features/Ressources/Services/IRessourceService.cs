using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.Query;

namespace Ressource_API.Features.Ressources.Services;

public interface IRessourceService
{
    Task<PaginatedList<ReturnRessourceDto>> GetAllRessourcesAsync(RessourceQuery ressourceQuery,
        CancellationToken cancellationToken = default);

    Task<ReturnRessourceDto> CreateRessourceAsync(CreateRessourceDto dto, ClaimsPrincipal context,
        CancellationToken token = default);

    Task<ReturnRessourceDto> UpdateRessourceAsync(Guid id, UpdateRessourceDto dto,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteRessourceAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> LikeRessource(Guid id, ClaimsPrincipal user);
}
