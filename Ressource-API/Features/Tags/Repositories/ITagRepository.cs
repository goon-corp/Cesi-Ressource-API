using Ressource_API.Features.Tags.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Tags.Query;

namespace Ressource_API.Features.Tags.Repositories;

public interface ITagRepository : IBaseRepository<Tag>
{
    Task<PaginatedList<Tag>> PaginatedListAsync(
        TagQuery query,
        CancellationToken cancellationToken = default);

}
