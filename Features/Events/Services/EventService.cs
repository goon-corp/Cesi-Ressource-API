using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Features.Events.Extensions;
using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Events.Query;
using Ressource_API.Features.Events.Repositories;
using Ressource_API.Features.Ressources.Extensions;
using Ressource_API.Features.Ressources.Repositories;
using Ressource_API.Features.Ressources.Services;
using Ressource_API.Features.UserRoles.Repositories;

namespace Ressource_API.Features.Events.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRessourceRepository _ressourceRepository;
    private readonly IRessourceService _ressourceService;
    private readonly IUserRoleRepository _userRoleRepository;

    private const string AdminRoleLabel = "Administrateur";

    public EventService(
        IEventRepository eventRepository,
        IRessourceService ressourceService,
        IRessourceRepository ressourceRepository,
        IUserRoleRepository userRoleRepository)
    {
        _eventRepository = eventRepository;
        _ressourceRepository = ressourceRepository;
        _ressourceService = ressourceService;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<Result<ReturnEventDto>> GetEventAsync(Guid ressourceId, CancellationToken token = default)
    {
        var existing = await _eventRepository.GetByRessourceIdWithIncludesAsync(ressourceId, token);
        if (existing is null)
            return Result.Failure<ReturnEventDto>("Event not found");

        return Result.Success(existing.ToReturnEventDto(existing.Ressource.ToReturnDto()));
    }

    public async Task<Result<ReturnEventDto>> CreateEventAsync(CreateEventDto createEventDto, ClaimsPrincipal context, CancellationToken token = default)
    {
        var createdRessource = await _ressourceService.CreateRessourceAsync(createEventDto.RessourceInfos, context);
        var eventToAdd = createEventDto.ToEvent(createdRessource.Id);
        Event createdEvent = await _eventRepository.AddAsync(eventToAdd);

        return Result.Success(createdEvent.ToReturnEventDto(createdRessource));
    }

    public async Task<Result<ReturnEventDto>> UpdateEventAsync(Guid eventId, UpdateEventDto updateEventDto, CancellationToken token = default)
    {
        var updatedRessource = await _ressourceService.UpdateRessourceAsync(updateEventDto.RessourceId, updateEventDto.Ressource, token);
        var existingEvent = await _eventRepository.FindAsync(eventId, token);

        if (existingEvent is null)
            return Result.Failure<ReturnEventDto>("Event not found");

        existingEvent.DateStart = updateEventDto.DateStart;
        existingEvent.DateEnd = updateEventDto.DateEnd;
        existingEvent.EventLink = updateEventDto.EventLink;
        existingEvent.Location = updateEventDto.Location;
        existingEvent.IsVirtual = updateEventDto.IsVirtual;
        existingEvent.RessourceId = updateEventDto.RessourceId;

        await _eventRepository.UpdateAsync(existingEvent, token);
        return Result.Success(existingEvent.ToReturnEventDto(updatedRessource));
    }

    public async Task<Result> DeleteEventAsync(Guid eventId, CancellationToken token = default)
    {
        var eventToDelete = await _eventRepository.FindAsync(eventId, token);

        if (eventToDelete is null)
            return Result.Failure("Event not found");

        var ressourceId = eventToDelete.RessourceId;

        await _eventRepository.RemoveAllMembersAsync(eventId, token);
        await _eventRepository.DeleteAsync(eventToDelete, token);

        var ressourceToDelete = await _ressourceRepository.FirstOrDefaultAsync(r => r.Id == ressourceId, token);
        if (ressourceToDelete is not null)
            await _ressourceRepository.DeleteAsync(ressourceToDelete, token);

        return Result.Success();
    }

    public async Task<Result<PaginatedList<ReturnEventMemberDto>>> GetEventMembersAsync(Guid eventId, EventMemberQuery query, CancellationToken token = default)
    {
        var eventExists = await _eventRepository.FindAsync(eventId, token);
        if (eventExists is null)
            return Result.Failure<PaginatedList<ReturnEventMemberDto>>("Event not found");

        var members = await _eventRepository.GetEventMembersAsync(eventId, query, token);
        return Result.Success(members);
    }

    public async Task<Result> AddMemberAsync(Guid eventId, Guid userId, ClaimsPrincipal context, CancellationToken token = default)
    {
        var eventEntity = await _eventRepository.GetByIdWithRessourceAsync(eventId, token);
        if (eventEntity is null)
            return Result.Failure("Event not found");

        var authResult = await IsAdminOrOrganizer(eventEntity, context, token);
        if (!authResult.IsSuccess)
            return authResult;

        var validationResult = ValidateEventNotPassed(eventEntity);
        if (!validationResult.IsSuccess)
            return validationResult;

        var isMember = await _eventRepository.IsMemberOfEventAsync(eventId, userId, token);
        if (isMember)
            return Result.Failure("User is already a member of this event");

        await _eventRepository.AddMemberAsync(eventId, userId, token);
        return Result.Success();
    }

    public async Task<Result> RemoveMemberAsync(Guid eventId, Guid userId, ClaimsPrincipal context, CancellationToken token = default)
    {
        var eventEntity = await _eventRepository.GetByIdWithRessourceAsync(eventId, token);
        if (eventEntity is null)
            return Result.Failure("Event not found");

        var authResult = await IsAdminOrOrganizer(eventEntity, context, token);
        if (!authResult.IsSuccess)
            return authResult;

        var isMember = await _eventRepository.IsMemberOfEventAsync(eventId, userId, token);
        if (!isMember)
            return Result.Failure("User is not a member of this event");

        await _eventRepository.RemoveMemberAsync(eventId, userId, token);
        return Result.Success();
    }

    public async Task<Result> JoinEventAsync(Guid eventId, ClaimsPrincipal context, CancellationToken token = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId is null)
            return Result.Failure("User not authenticated");

        var eventEntity = await _eventRepository.FindAsync(eventId, token);
        if (eventEntity is null)
            return Result.Failure("Event not found");

        var validationResult = ValidateEventNotPassed(eventEntity);
        if (!validationResult.IsSuccess)
            return validationResult;

        var isMember = await _eventRepository.IsMemberOfEventAsync(eventId, userId.Value, token);
        if (isMember)
            return Result.Failure("You are already a member of this event");

        await _eventRepository.AddMemberAsync(eventId, userId.Value, token);
        return Result.Success();
    }

    public async Task<Result> LeaveEventAsync(Guid eventId, ClaimsPrincipal context, CancellationToken token = default)
    {
        var userId = GetUserIdFromContext(context);
        if (userId is null)
            return Result.Failure("User not authenticated");

        var eventEntity = await _eventRepository.FindAsync(eventId, token);
        if (eventEntity is null)
            return Result.Failure("Event not found");

        var isMember = await _eventRepository.IsMemberOfEventAsync(eventId, userId.Value, token);
        if (!isMember)
            return Result.Failure("You are not a member of this event");

        await _eventRepository.RemoveMemberAsync(eventId, userId.Value, token);
        return Result.Success();
    }

    private static Guid? GetUserIdFromContext(ClaimsPrincipal context)
    {
        var userIdClaim = context.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return null;
        return userId;
    }

    private async Task<Result> IsAdminOrOrganizer(Event eventEntity, ClaimsPrincipal context, CancellationToken token)
    {
        var userId = GetUserIdFromContext(context);
        if (userId is null)
            return Result.Failure("User not authenticated");

        var userRoleId = context.FindFirstValue(ClaimTypes.Role);
        if (!string.IsNullOrEmpty(userRoleId) && Guid.TryParse(userRoleId, out var roleId))
        {
            var role = await _userRoleRepository.FindAsync(roleId, token);
            if (role?.RoleLabel == AdminRoleLabel)
                return Result.Success();
        }

        if (eventEntity.Ressource?.UserId == userId.Value)
            return Result.Success();

        return Result.Failure("You are not authorized to perform this action");
    }

    private static Result ValidateEventNotPassed(Event eventEntity)
    {
        var today = DateTime.UtcNow.Date;
        if (eventEntity.DateEnd.Date < today)
            return Result.Failure("Cannot join or modify a past event");

        return Result.Success();
    }
}
