using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.TagDtos;

namespace Ressource_API.Features.Tags.Services;

public interface ITagService
{
    Task<IEnumerable<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default);
    Task<Tag?> GetTagByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Tag> CreateTagAsync(CreateTagDto dto, CancellationToken cancellationToken = default);
    Task<Tag?> UpdateTagAsync(int id, UpdateTagDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteTagAsync(int id, CancellationToken cancellationToken = default);
}
