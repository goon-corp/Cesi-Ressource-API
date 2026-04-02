using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Models;

namespace Ressource_API.Features.Polls.Factories;

public interface IPollFactory
{
    Poll Create(CreatePollDto dto, Guid ressourceId);
}