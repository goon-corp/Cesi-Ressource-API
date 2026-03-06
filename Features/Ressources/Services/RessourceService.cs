using Microsoft.Extensions.Caching.Hybrid;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.RessourceDtos;
using Ressource_API.Features.Ressources.Repositories;
using Ressource_API.Features.Ressources.Factories;
using Ressource_API.Features.Ressources.Query;

namespace Ressource_API.Features.Ressources.Services;

public class RessourceService : IRessourceService
{
    private readonly IRessourceRepository _repository;
    private readonly IRessourceFactory _factory;
    private readonly HybridCache _cache;

    public RessourceService(
        IRessourceRepository repository,
        IRessourceFactory factory,
        HybridCache cache)
    {
        _repository = repository;
        _factory = factory;
        _cache = cache;
    }

    public async Task<PaginatedList<Ressource>> GetAllRessourcesAsync(RessourceQuery ressourceQuery, CancellationToken cancellationToken = default)
    {
        var ressourcesTask = _repository.PaginatedRessourcesAsync(ressourceQuery, cancellationToken);
        if (!string.IsNullOrWhiteSpace(ressourceQuery.RessourceType) ||
            ressourceQuery.CreatedAt.HasValue ||
            ressourceQuery.IsDeleted.HasValue ||
            ressourceQuery.RessourceTitle is not null)
            return await ressourcesTask;

        var isComplete = true;

        var cacheKey = $"ressources:p={ressourceQuery.page}:s={ressourceQuery.size}";
        var incompleteRessources = new List<string> { "ressources:incompletePage" };
        var completeRessources = new List<string> { "ressources:completePage" };
        var entryOptions = new HybridCacheEntryOptions();

        var ressources = await _cache.GetOrCreateAsync(cacheKey, async _ =>
            {
                var ressources = await ressourcesTask;
                isComplete = ressources.Count == ressourceQuery.size;

                await TagCacheHandler(ressources, ressourceQuery, cacheKey);

                return ressources;
            },
            entryOptions,
            isComplete ? completeRessources : incompleteRessources,
            cancellationToken);

        return ressources;
    }

    public async Task TagCacheHandler(PaginatedList<Ressource> ressources, RessourceQuery ressourceQuery, string cacheKey)
    {
        foreach (var ressource in ressources)
        {
            var ressourcePagesCacheKey = $"invert:ressources:{ressource.Id}";

            var invertedRessourcesPages = await _cache.GetOrCreateAsync(ressourcePagesCacheKey,
                async _ => { return await Task.FromResult(new HashSet<string>()); });
            invertedRessourcesPages.Add(cacheKey);
            await _cache.SetAsync(ressourcePagesCacheKey, invertedRessourcesPages);
        }
    }

    public async Task<Ressource?> GetRessourceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _cache.GetOrCreateAsync($"ressources:{id}", async _ =>
            await _repository.FindAsync(id, cancellationToken));

        return existing;
    }

    public async Task<Ressource> CreateRessourceAsync(CreateRessourceDto dto, CancellationToken cancellationToken = default)
    {
        var ressource = _factory.Create(dto);

        await _repository.AddAsync(ressource, cancellationToken);
        await _cache.SetAsync($"ressources:{ressource.Id}", ressource);
        await _cache.RemoveByTagAsync("ressources:incompletePage", cancellationToken);

        return ressource;
    }

    public async Task<Ressource?> UpdateRessourceAsync(int id, UpdateRessourceDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);

        if (existing == null)
            return null;

        // TODO: Map properties from dto to existing
        // Example: existing.Name = dto.Name;

        await _repository.UpdateAsync(existing, cancellationToken);
        await _cache.SetAsync($"ressources:{id}", existing);

        await UpdateAffectedPages(existing);

        return existing;
    }

    public async Task<bool> DeleteRessourceAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);

        if (existing == null)
            return false;

        var existingPages = await _cache.GetOrCreateAsync($"invert:ressources:{id}",
            async _ => { return await Task.FromResult(new HashSet<string>()); });

        foreach (var page in existingPages)
            await _cache.RemoveAsync(page, cancellationToken);

        await _repository.DeleteAsync(existing, cancellationToken);
        await _cache.RemoveAsync($"ressources:{id}");
        await _cache.RemoveAsync($"invert:ressources:{id}");

        return true;
    }

    private async Task UpdateAffectedPages(Ressource ressource)
    {
        var affectedPagesKey = $"invert:ressources:{ressource.Id}";
        var affectedPageKeys = await _cache.GetOrCreateAsync(affectedPagesKey,
            async _ => { return await Task.FromResult(new HashSet<string>()); });

        foreach (var pageKey in affectedPageKeys)
        {
            var page = await _cache.GetOrCreateAsync(pageKey,
                async _ => { return await Task.FromResult(new PaginatedList<Ressource>()); });

            UpdateRessourceInPage(ressource, page);
            await _cache.SetAsync(pageKey, page);
        }
    }

    private void UpdateRessourceInPage(Ressource existing, PaginatedList<Ressource> page)
    {
        foreach (var ressource in page)
            if (existing.Id == ressource.Id)
            {
                ressource.RessourceType = existing.RessourceType;
                ressource.CreationTime = existing.CreationTime;
                ressource.Title = existing.Title;
                ressource.Description = existing.Description;
                ressource.Articles = existing.Articles;
                ressource.DeletionTime = existing.DeletionTime;
                ressource.Events = existing.Events;
                ressource.FavoritedByUsers = existing.FavoritedByUsers;
                ressource.Comments = existing.Comments;
                ressource.UpdateTime = DateTime.UtcNow; 
                ressource.DeletionTime = existing.DeletionTime;
                ressource.Tags = existing.Tags;
                ressource.RessourceConfidentialityType = existing.RessourceConfidentialityType;
                ressource.RessourceStatusId = existing.RessourceStatusId;
                ressource.Polls = existing.Polls;
                ressource.Quizzes = existing.Quizzes;
                ressource.RessourceProgressions = existing.RessourceProgressions;
                ressource.ThumbnailUrl = existing.ThumbnailUrl;
                ressource.UserId = existing.UserId;
                ressource.ViewCount = existing.ViewCount;
                ressource.RessourceConfidentialityTypeId = existing.RessourceConfidentialityTypeId;
                ressource.RessourceTypeId = existing.RessourceTypeId;
                ressource.RessourceStatus = existing.RessourceStatus;
                ressource.User =  existing.User;
                ressource.LikedByUsers = existing.LikedByUsers;
                ressource.Sessions = existing.Sessions;
                ressource.RessourcesMedia = existing.RessourcesMedia;
                ressource.Reports = existing.Reports;
            }
    }
}