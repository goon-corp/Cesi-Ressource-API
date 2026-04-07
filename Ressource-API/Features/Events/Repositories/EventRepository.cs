using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Events.Query;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.Events.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Event?> GetByRessourceIdWithIncludesAsync(Guid ressourceId, CancellationToken cancellationToken = default)
    {
        return await _context.Events
            .AsNoTracking()
            .Include(e => e.Ressource)
                .ThenInclude(r => r.RessourceStatus)
            .Include(e => e.Ressource)
                .ThenInclude(r => r.RessourceConfidentialityType)
            .Include(e => e.Ressource)
                .ThenInclude(r => r.RessourceType)
            .Include(e => e.Ressource)
                .ThenInclude(r => r.Tags)
            .FirstOrDefaultAsync(e => e.RessourceId == ressourceId, cancellationToken);
    }

    public async Task<Event?> GetByIdWithMembersAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _context.Events
            .Include(e => e.Users)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
    }

    public async Task<Event?> GetByIdWithRessourceAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _context.Events
            .Include(e => e.Ressource)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
    }

    public async Task<PaginatedList<ReturnEventMemberDto>> GetEventMembersAsync(Guid eventId, EventMemberQuery query, CancellationToken cancellationToken = default)
    {
        var membersQuery = _context.Events
            .Where(e => e.Id == eventId)
            .SelectMany(e => e.Users)
            .Where(u => u.DeletionTime == null)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.UserName))
        {
            membersQuery = membersQuery.Where(u => u.UserName.Contains(query.UserName));
        }

        var totalCount = await membersQuery.CountAsync(cancellationToken);

        var members = await membersQuery
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .Select(u => new ReturnEventMemberDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<ReturnEventMemberDto>(members, query.page, query.size, totalCount);
    }

    public async Task<bool> IsMemberOfEventAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Events
            .Where(e => e.Id == eventId)
            .SelectMany(e => e.Users)
            .AnyAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task AddMemberAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Users)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        if (eventEntity is null)
            throw new KeyNotFoundException("Event not found");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("User not found");

        eventEntity.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveMemberAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Users)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        if (eventEntity is null)
            throw new KeyNotFoundException("Event not found");

        var user = eventEntity.Users.FirstOrDefault(u => u.Id == userId);

        if (user is not null)
        {
            eventEntity.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RemoveAllMembersAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Users)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        if (eventEntity is null)
            throw new KeyNotFoundException("Event not found");

        eventEntity.Users.Clear();
        await _context.SaveChangesAsync(cancellationToken);
    }
}
