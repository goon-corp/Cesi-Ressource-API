using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.PollOptions.Query;

namespace Ressource_API.Features.PollOptions.Repositories;

public interface IPollOptionRepository : IBaseRepository<PollOption>
{
    Task<PaginatedList<PollOptionInfoDto>> PaginatedPollOptionsAsync(
        PollOptionQuery query,
        CancellationToken cancellationToken = default);

    Task<PollOption?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}