using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.PollOptions.PollOptionDtos;

namespace Ressource_API.Features.PollOptions.Services;

public interface IPollOptionService
{
    Task<IEnumerable<PollOption>> GetAllPollOptionsAsync(CancellationToken cancellationToken = default);
    Task<PollOption?> GetPollOptionByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PollOption> CreatePollOptionAsync(CreatePollOptionDto dto, CancellationToken cancellationToken = default);
    Task<PollOption?> UpdatePollOptionAsync(int id, UpdatePollOptionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeletePollOptionAsync(int id, CancellationToken cancellationToken = default);
}
