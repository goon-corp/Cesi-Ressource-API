using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Events.Extensions;

public static class EventExtensions
{
    extension(CreateEventDto eventToCreate) 
    {
        public Event ToEvent(Guid ressourceId)
        {
            return new Event()
            {
                IsVirtual = eventToCreate.IsVirtual,
                DateStart = eventToCreate.DateStart,
                DateEnd = eventToCreate.DateEnd,
                EventLink = eventToCreate.EventLink,
                Location = eventToCreate.Location,
                RessourceId = ressourceId
            };
        }
    }

    extension(Event _event)
    {
        public ReturnEventDto ToReturnEventDto(ReturnRessourceDto ressource)
        {
            return new ReturnEventDto()
            {
                Id = _event.Id,
                IsVirtual = _event.IsVirtual,
                DateStart = _event.DateStart,
                DateEnd = _event.DateEnd,
                EventLink = _event.EventLink,
                Location = _event.Location,
                Ressource = ressource
            };
        }
    }
}