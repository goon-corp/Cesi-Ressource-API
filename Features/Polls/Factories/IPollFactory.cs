using Ressource_API.Features.Polls.Models;
using Ressource_API.Features.Polls.PollDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Polls.Factories;

public interface IPollFactory : IBaseFactory<Poll>
{
    Poll Create(CreatePollDto dto);
}
