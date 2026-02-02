using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.PollOptions.PollOptionDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.PollOptions.Factories;

public interface IPollOptionFactory : IBaseFactory<PollOption>
{
    PollOption Create(CreatePollOptionDto dto);
}
