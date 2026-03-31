using Ressource_API.Common.Pagination;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Features.Events.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Features.Events.Query;

namespace Ressource_API.Features.Events.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<Event?> GetByRessourceIdWithIncludesAsync(Guid ressourceId, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdWithMembersAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdWithRessourceAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<PaginatedList<ReturnEventMemberDto>> GetEventMembersAsync(Guid eventId, EventMemberQuery query, CancellationToken cancellationToken = default);
    Task<bool> IsMemberOfEventAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default);
    Task AddMemberAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default);
    Task RemoveMemberAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default);
    Task RemoveAllMembersAsync(Guid eventId, CancellationToken cancellationToken = default);
}
