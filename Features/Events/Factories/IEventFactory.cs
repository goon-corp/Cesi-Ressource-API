using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Events.Factories;

public interface IEventFactory : IBaseFactory<Event>
{
    Event Create(CreateEventDto dto);
}
