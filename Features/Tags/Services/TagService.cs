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
    private readonly ITagRepository _repository;

    private readonly HybridCache _cache;

    public TagService(ITagRepository repository, HybridCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task TagCacheHandler(PaginatedList<Tag> tags, TagQuery tagQuery, string cacheKey)
    {
        foreach (var tag in tags)
        {
            var tagPagesCacheKey = $"invert:tags:{tag.Id}";

            var invertedTagsPages = await _cache.GetOrCreateAsync(tagPagesCacheKey,
                async _ => { return await Task.FromResult(new HashSet<string>()); });
            invertedTagsPages.Add(cacheKey);
            await _cache.SetAsync(tagPagesCacheKey, invertedTagsPages);
        }
    }

    public async Task<PaginatedList<Tag>> GetAllTagsAsync(
        TagQuery tagQuery,
        CancellationToken cancellationToken = default)
    {
        var tagsTask = _repository.PaginatedListAsync(tagQuery, cancellationToken);

        if (!string.IsNullOrWhiteSpace(tagQuery.TagName) ||
            tagQuery.CreatedAt.HasValue ||
            tagQuery.IsDeleted.HasValue)
            return await tagsTask;

        var isComplete = true;

        var cacheKey = $"tags:p={tagQuery.page}:s={tagQuery.size}";
        var incompleteTags = new List<string> { "incompletePage" };
        var completeTags = new List<string> { "completePage" };
        var entryOptions = new HybridCacheEntryOptions();

        var tags = await _cache.GetOrCreateAsync(cacheKey, async _ =>
            {
                var tags = await tagsTask;
                isComplete = tags.Count == tagQuery.size;
                
                await TagCacheHandler(tags, tagQuery, cacheKey);
                
                return tags;
            },
            entryOptions,
            isComplete ? completeTags : incompleteTags,
            cancellationToken);
        
        return tags;
    }

    public async Task<bool> DeleteTagAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await GetTagByIdAsync(id, cancellationToken);

        existing?.DeletionTime = DateTime.UtcNow;

        if (existing != null) await _repository.SoftDeleteAsync(existing, cancellationToken);

        return true;
    }


    public async Task<Tag?> GetTagByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _cache.GetOrCreateAsync($"tags:{id}", async _ =>
        {
            return await _repository.FindAsync(id, cancellationToken);
            
        });
    return  existing;
    }

    public async Task<Tag> CreateTagAsync(CreateTagDto dto, CancellationToken cancellationToken = default)
    {
        // Utiliser la methode d'extension pour map les DTOs
        var tag = dto.ToModel();

        return await _repository.AddAsync(tag, cancellationToken);
    }

    public async Task<Tag?> UpdateTagAsync(Guid id, UpdateTagDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);

        if (existing == null) return null;

        // Map properties from dto to existing
        existing.Label = dto.Label;
        existing.UpdateTime = DateTime.UtcNow; // Mise à jour automatique
        existing.DeletionTime = dto.DeletionTime;
        // existing.CreationTime ne devrait PAS être modifié

        await _repository.UpdateAsync(existing, cancellationToken);

        return existing;
    }
}