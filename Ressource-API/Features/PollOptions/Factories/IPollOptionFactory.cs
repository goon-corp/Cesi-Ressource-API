using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.PollOptions.Models;

namespace Ressource_API.Features.PollOptions.Factories;

public interface IPollOptionFactory
{
    PollOption Create(CreatePollOptionDto dto);
}