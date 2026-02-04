using Microsoft.Extensions.Caching.Hybrid;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Tags.Extensions;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.TagDtos;
using Ressource_API.Features.Tags.Repositories;
using Ressource_API.Features.Tags.Factories;
using Ressource_API.Features.Tags.Query;

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

    public async Task<PaginatedList<Tag>> GetAllTagsAsync(
        TagQuery tagQuery,
        CancellationToken cancellationToken = default)
    {
        var tags = await _cache.GetOrCreateAsync(
            "tags",
            async token => await _repository.ListAsync(token)
        );

        IEnumerable<Tag> query = tags;
        

        if (!string.IsNullOrWhiteSpace(tagQuery.TagName))
        {
            query = query.Where(t =>
                t.Label.Contains(tagQuery.TagName, StringComparison.OrdinalIgnoreCase));
        }

        if (tagQuery.CreatedAt.HasValue)
        {
            query = query.Where(t =>
                t.CreationTime.Date == tagQuery.CreatedAt.Value.Date);
        }

        if (tagQuery.IsDeleted is not null)
        {

            query = (bool)tagQuery.IsDeleted
                ? query.Where(t => t.DeletionTime != null)
                : query.Where(t => t.DeletionTime == null);
        }
        

        var filteredList = query.ToList();

        var paginatedTags = new PaginatedList<Tag>(
            filteredList,
            tagQuery.page,
            tagQuery.size
        );

        return paginatedTags;
    }


    public async Task<Tag?> GetTagByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
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

        if (existing == null)
        {
            return null;
        }

        // Map properties from dto to existing
        existing.Label = dto.Label;
        existing.UpdateTime = DateTime.UtcNow; // Mise à jour automatique
        existing.DeletionTime = dto.DeletionTime;
        // existing.CreationTime ne devrait PAS être modifié

        await _repository.UpdateAsync(existing, cancellationToken);

        return existing;
    }
    public async Task<bool> DeleteTagAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);

        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);

        return true;
    }
}