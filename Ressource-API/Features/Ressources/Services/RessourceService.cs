using System.Security.Claims;
using Microsoft.Extensions.Caching.Hybrid;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.RessourceMedias.Dtos;
using Ressource_API.Features.RessourceMedias.Services;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Extensions;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.Repositories;
using Ressource_API.Features.Ressources.Query;
using Ressource_API.Features.RessourceConfidentialityTypes.Repositories;
using Ressource_API.Features.RessourceStatuses.Repositories;
using Ressource_API.Features.RessourceTypes.Repositories;
using Ressource_API.Features.Tags.Repositories;

namespace Ressource_API.Features.Ressources.Services;

public class RessourceService : IRessourceService
{
    private readonly IRessourceRepository _repository;
    private readonly HybridCache _cache;
    private readonly IRessourceMediaService _mediaService;
    private readonly ITagRepository _tagRepository;
    private readonly IRessourceStatusRepository _statusRepository;
    private readonly IRessourceConfidentialityTypeRepository _confidentialityTypeRepository;
    private readonly IRessourceTypeRepository _typeRepository;

    public RessourceService(
        IRessourceRepository repository,
        HybridCache cache,
        IRessourceMediaService mediaService,
        ITagRepository tagRepository,
        IRessourceStatusRepository statusRepository,
        IRessourceConfidentialityTypeRepository confidentialityTypeRepository,
        IRessourceTypeRepository typeRepository)
    {
        _repository = repository;
        _cache = cache;
        _mediaService = mediaService;
        _tagRepository = tagRepository;
        _statusRepository = statusRepository;
        _confidentialityTypeRepository = confidentialityTypeRepository;
        _typeRepository = typeRepository;
    }

    public async Task<PaginatedList<ReturnRessourceDto>> GetAllRessourcesAsync(RessourceQuery ressourceQuery,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(ressourceQuery.RessourceType) ||
            ressourceQuery.RessourceTags is { Count: > 0 } ||
            ressourceQuery.CreatedAt.HasValue ||
            ressourceQuery.IsDeleted.HasValue ||
            ressourceQuery.RessourceTitle is not null)
            return await _repository.PaginatedRessourcesAsync(ressourceQuery, cancellationToken);

        var cacheKey = $"ressources:p={ressourceQuery.page}:s={ressourceQuery.size}";
        var entryOptions = new HybridCacheEntryOptions();

        PaginatedList<ReturnRessourceDto>? freshRessources = null;

        var ressources = await _cache.GetOrCreateAsync(cacheKey, async _ =>
            {
                freshRessources = await _repository.PaginatedRessourcesAsync(ressourceQuery, cancellationToken);
                return freshRessources;
            },
            entryOptions,
            ["ressources:completePage"],
            cancellationToken);

        if (freshRessources is not null)
            await RessourceCacheHandler(freshRessources, ressourceQuery, cacheKey);

        return ressources;
    }

    public async Task RessourceCacheHandler(PaginatedList<ReturnRessourceDto> ressources, RessourceQuery ressourceQuery,
        string cacheKey)
    {
        foreach (var ressource in ressources.Items)
        {
            var ressourcePagesCacheKey = $"invert:ressources:{ressource.Id}";

            var invertedRessourcesPages = await _cache.GetOrCreateAsync(ressourcePagesCacheKey,
                async _ => { return await Task.FromResult(new HashSet<string>()); });
            invertedRessourcesPages.Add(cacheKey);
            await _cache.SetAsync(ressourcePagesCacheKey, invertedRessourcesPages);
        }
    }

    public async Task<ReturnRessourceDto> CreateRessourceAsync(CreateRessourceDto dto, ClaimsPrincipal context,
        CancellationToken token = default)
    {
        var authorId = context.FindFirstValue(ClaimTypes.NameIdentifier);
        if (authorId is null) throw new NullReferenceException("Couldnt find author");

        Guid? thumbnailId = null;
        if (dto.Thumbnail is not null)
        {
            var media = await _mediaService.CreateMedia(new CreateRessourceMediaDto() { File = dto.Thumbnail });
            thumbnailId = media.Id;
        }

        var tags = dto.Tags.Any()
            ? await _tagRepository.ListAsync(t => dto.Tags.Contains(t.Id), token)
            : [];

        var status = await _statusRepository.FindAsync(dto.StatusId, token);
        var confidentialityType = await _confidentialityTypeRepository.FindAsync(dto.ConfidentialityTypeId, token);
        var type = await _typeRepository.FindAsync(dto.TypeId, token);

        var ressource = new Ressource()
        {
            Id = Guid.CreateVersion7(),
            UserId = Guid.Parse(authorId),
            Title = dto.Title,
            CreationTime = DateTime.UtcNow,
            Description = dto.Description,
            ThumbnailId = thumbnailId,
            RessourceStatusId = dto.StatusId,
            RessourceConfidentialityTypeId = dto.ConfidentialityTypeId,
            RessourceTypeId = dto.TypeId,
            Tags = tags,
            RessourceStatus = status!,
            RessourceConfidentialityType = confidentialityType!,
            RessourceType = type!,
        };

        await _repository.AddAsync(ressource, token);
        await _cache.RemoveByTagAsync("ressources:incompletePage", token);

        return ressource.ToReturnDto();
    }

    public async Task<ReturnRessourceDto> UpdateRessourceAsync(Guid id, UpdateRessourceDto dto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindWithTagsAsync(id, cancellationToken);
        if (existing is null) throw new KeyNotFoundException("Cannot find Ressource");

        var tags = dto.Tags.Any()
            ? await _tagRepository.ListAsync(t => dto.Tags.Contains(t.Id), cancellationToken)
            : [];

        var status = await _statusRepository.FindAsync(dto.StatusId, cancellationToken);
        var confidentialityType = await _confidentialityTypeRepository.FindAsync(dto.ConfidentialityTypeId, cancellationToken);
        var type = await _typeRepository.FindAsync(dto.TypeId, cancellationToken);

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.UpdateTime = DateTime.UtcNow;
        existing.RessourceStatusId = dto.StatusId;
        existing.RessourceStatus = status!;
        existing.RessourceConfidentialityTypeId = dto.ConfidentialityTypeId;
        existing.RessourceConfidentialityType = confidentialityType!;
        existing.RessourceTypeId = dto.TypeId;
        existing.RessourceType = type!;
        existing.Tags = tags;

        await _repository.UpdateAsync(existing, cancellationToken);
        await _cache.RemoveAsync($"ressources:{id}", cancellationToken);
        await UpdateAffectedPages(existing, cancellationToken);

        return existing.ToReturnDto();
    }

    public async Task<bool> DeleteRessourceAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        if (existing is null) return false;

        var affectedPageKeys = await _cache.GetOrCreateAsync($"invert:ressources:{id}",
            async _ => await Task.FromResult(new HashSet<string>()));

        foreach (var page in affectedPageKeys)
            await _cache.RemoveAsync(page, cancellationToken);

        await _repository.DeleteAsync(existing, cancellationToken);
        await _cache.RemoveAsync($"ressources:{id}", cancellationToken);
        await _cache.RemoveAsync($"invert:ressources:{id}", cancellationToken);

        return true;
    }

    public async Task<Result> LikeRessource(Guid id, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Result.Failure("User not authenticated");

        var toggled = await _repository.ToggleLikeAsync(id, Guid.Parse(userId));
        if (toggled is null) return Result.Failure("Ressource not found");

        return Result.Success();
    }

    public async Task<Result> FavoriteRessource(Guid id, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Result.Failure("User not authenticated");

        var toggled = await _repository.ToggleFavoriteAsync(id, Guid.Parse(userId));
        if (toggled is null) return Result.Failure("Ressource not found");

        return Result.Success();
    }

    public async Task<Result<RessourceUserStatusDto>> GetUserStatus(Guid id, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Result.Failure<RessourceUserStatusDto>("User not authenticated");

        var status = await _repository.GetUserStatusAsync(id, Guid.Parse(userId));
        if (status is null) return Result.Failure<RessourceUserStatusDto>("Ressource not found");

        return Result.Success(status);
    }

    private async Task UpdateAffectedPages(Ressource ressource, CancellationToken cancellationToken = default)
    {
        var updated = ressource.ToReturnDto();

        var affectedPageKeys = await _cache.GetOrCreateAsync($"invert:ressources:{ressource.Id}",
            async _ => await Task.FromResult(new HashSet<string>()), cancellationToken: cancellationToken);

        foreach (var pageKey in affectedPageKeys)
        {
            var page = await _cache.GetOrCreateAsync(pageKey,
                async _ => await Task.FromResult(new PaginatedList<ReturnRessourceDto>(new List<ReturnRessourceDto>(), 1, 1, 0)), cancellationToken: cancellationToken);

            var entry = page.Items.FirstOrDefault(r => r.Id == ressource.Id);
            if (entry is not null)
            {
                entry.Title = updated.Title;
                entry.Description = updated.Description;
                entry.ThumbnailId = updated.ThumbnailId;
                entry.Status = updated.Status;
                entry.ConfidentialityType = updated.ConfidentialityType;
                entry.Type = updated.Type;
                entry.Tags = updated.Tags;
            }

            await _cache.SetAsync(pageKey, page, cancellationToken: cancellationToken);
        }
    }
}