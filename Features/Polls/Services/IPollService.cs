using Ressource_API.Features.Polls.Models;
using Ressource_API.Features.Polls.PollDtos;

namespace Ressource_API.Features.Polls.Services;

public interface IPollService
{
    Task<IEnumerable<Poll>> GetAllPollsAsync(CancellationToken cancellationToken = default);
    Task<Poll?> GetPollByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Poll> CreatePollAsync(CreatePollDto dto, CancellationToken cancellationToken = default);
    Task<Poll?> UpdatePollAsync(int id, UpdatePollDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeletePollAsync(int id, CancellationToken cancellationToken = default);
}
