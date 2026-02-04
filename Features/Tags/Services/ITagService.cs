using Ressource_API.Common.Pagination;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.Query;
using Ressource_API.Features.Tags.TagDtos;

namespace Ressource_API.Features.Tags.Services;

public interface ITagService
{
    Task<PaginatedList<Tag>> GetAllTagsAsync(TagQuery tagQuery, CancellationToken cancellationToken = default);
    Task<Tag?> GetTagByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Tag> CreateTagAsync(CreateTagDto dto, CancellationToken cancellationToken = default);
    Task<Tag?> UpdateTagAsync(Guid id, UpdateTagDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteTagAsync(Guid id, CancellationToken cancellationToken = default);
}
