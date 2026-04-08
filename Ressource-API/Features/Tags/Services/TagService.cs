using Microsoft.Extensions.Caching.Hybrid;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Tags.Extensions;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.Query;
using Ressource_API.Features.Tags.Repositories;
using Ressource_API.Features.Tags.TagDtos;

namespace Ressource_API.Features.Tags.Services;

public class TagService : ITagService
{
    private readonly HybridCache _cache;
    private readonly ITagRepository _repository;
    private readonly ILogger<TagService> _logger;

    public TagService(ITagRepository repository, HybridCache cache,  ILogger<TagService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<PaginatedList<Tag>> GetAllTagsAsync(
        TagQuery tagQuery,
        CancellationToken cancellationToken = default)
    {
        return await _repository.PaginatedListAsync(tagQuery, cancellationToken);
    }

    public async Task DeleteTagAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);

        if (existing is null)
            throw new KeyNotFoundException($"Tag with id '{id}' was not found.");

        existing.DeletionTime = DateTime.UtcNow;
        await _repository.SoftDeleteAsync(existing, cancellationToken);
    }

    public async Task<Tag> GetTagByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FirstOrDefaultAsyncAsNoTracking(x => x.Id == id, cancellationToken);

        if (existing is null)
            throw new KeyNotFoundException($"Tag with id '{id}' was not found.");

        return existing;
    }

    public async Task<Tag> CreateTagAsync(CreateTagDto dto, CancellationToken cancellationToken = default)
    {
        var tag = dto.ToModel();
        await _repository.AddAsync(tag, cancellationToken);
        return tag;
    }

    public async Task<Tag> UpdateTagAsync(Guid id, UpdateTagDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        if (existing == null) throw new KeyNotFoundException($"Tag with id '{id}' was not found.");

        existing.Label = dto.Label;
        existing.UpdateTime = DateTime.UtcNow;
        existing.DeletionTime = dto.DeletionTime;

        await _repository.UpdateAsync(existing, cancellationToken);

        return existing;
    }
}
