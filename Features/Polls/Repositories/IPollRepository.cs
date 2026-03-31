using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Models;
using Ressource_API.Features.Polls.Query;

namespace Ressource_API.Features.Polls.Repositories;

public interface IPollRepository : IBaseRepository<Poll>
{
    Task<PaginatedList<PollInfoDto>> PaginatedPollsAsync(
        PollQuery query,
        CancellationToken cancellationToken = default);

    Task<Poll?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}