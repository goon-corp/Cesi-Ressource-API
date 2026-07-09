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
    private readonly  ILogger<TagService> _logger;

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
        if (!string.IsNullOrWhiteSpace(tagQuery.TagName) ||
            tagQuery.CreatedAt.HasValue ||
            tagQuery.IsDeleted.HasValue)
            return await _repository.PaginatedListAsync(tagQuery, cancellationToken);;

        var isComplete = true;

        var cacheKey = $"tags:p={tagQuery.page}:s={tagQuery.size}";
        var incompleteTags = new List<string> { "tags:incompletePage" };
        var completeTags = new List<string> { "tags:completePage" };
        var entryOptions = new HybridCacheEntryOptions();

        var tags = await _cache.GetOrCreateAsync(cacheKey, async _ =>
            {
                var tags = await _repository.PaginatedListAsync(tagQuery, cancellationToken);;
                isComplete = tags.Items.Count == tagQuery.size;

                await TagCacheHandler(tags, tagQuery, cacheKey);

                return tags;
            },
            entryOptions,
            isComplete ? completeTags : incompleteTags,
            cancellationToken);

        return tags;
    }

    public async Task DeleteTagAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);

        if (existing is null)
            throw new KeyNotFoundException($"Tag with id '{id}' was not found.");

        var existingPages = await _cache.GetOrCreateAsync($"invert:tags:{id}",
            async _ => { return await Task.FromResult(new HashSet<string>()); });

        foreach (var page in existingPages)
            await _cache.RemoveAsync(page, cancellationToken);

        existing.DeletionTime = DateTime.UtcNow;
        await _repository.SoftDeleteAsync(existing, cancellationToken);
        await _cache.RemoveAsync($"tags:{id}");
        await _cache.RemoveAsync($"invert:tags:{id}");
    }

    public async Task<Tag> GetTagByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _cache.GetOrCreateAsync($"tags:{id}", async _ =>
            await _repository.FirstOrDefaultAsyncAsNoTracking(x => x.Id == id, cancellationToken));

        if (existing is null)
            throw new KeyNotFoundException($"Tag with id '{id}' was not found.");

        return existing;
    }

    public async Task<Tag> CreateTagAsync(CreateTagDto dto, CancellationToken cancellationToken = default)
    {
        var tag = dto.ToModel();
        await _repository.AddAsync(tag, cancellationToken);
        var tagKey = $"tags:{tag.Id}";
        await _cache.SetAsync(tagKey, tag);
        await _cache.RemoveByTagAsync("tags:incompletePage", cancellationToken);
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
        await _cache.SetAsync($"tags:{id}", existing);

        await UpdateAffectedPages(existing); 

        return existing;
    }

    public async Task TagCacheHandler(PaginatedList<Tag> tags, TagQuery tagQuery, string cacheKey)
    {
        foreach (var tag in tags.Items)
        {
            var tagPagesCacheKey = $"invert:tags:{tag.Id}";

            var invertedTagsPages = await _cache.GetOrCreateAsync(tagPagesCacheKey,
                async _ => { return await Task.FromResult(new HashSet<string>()); });
            invertedTagsPages.Add(cacheKey);
            await _cache.SetAsync(tagPagesCacheKey, invertedTagsPages);
        }
    }

    private async Task UpdateAffectedPages(Tag tag)
    {
        var affectedPagesKey = $"invert:tags:{tag.Id}";
        var affectedPageKeys = await _cache.GetOrCreateAsync(affectedPagesKey,
            async _ => { return await Task.FromResult(new HashSet<string>()); });

        foreach (var pageKey in affectedPageKeys)
        {
            var page = await _cache.GetOrCreateAsync(pageKey,
                async _ => { return await Task.FromResult(new PaginatedList<Tag>(new List<Tag>(), 1, 1, 0)); });

            UpdateTagInPage(tag, page);
            await _cache.SetAsync(pageKey, page);
        }
    }

    private void UpdateTagInPage(Tag existing, PaginatedList<Tag> page)
    {
        foreach (var tag in page.Items)
            if (existing.Id == tag.Id)
            {
                tag.Label = existing.Label;
                tag.UpdateTime = DateTime.UtcNow;
                tag.DeletionTime = existing.DeletionTime;
            }
    }
}