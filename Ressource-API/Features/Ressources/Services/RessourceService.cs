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

    public async Task<ReturnRessourceDto?> GetRessourceByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"ressources:{id}";

        var ressource = await _cache.GetOrCreateAsync(cacheKey, async _ =>
            await _repository.GetRessourceByIdAsync(id, cancellationToken),
            cancellationToken: cancellationToken);

        return ressource;
    }

    public async Task<PaginatedList<ReturnRessourceDto>> GetAllRessourcesAsync(RessourceQuery ressourceQuery, CancellationToken cancellationToken = default)
    {

        return await _repository.PaginatedRessourcesAsync(ressourceQuery, cancellationToken);
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

        return existing.ToReturnDto();
    }

    public async Task<bool> DeleteRessourceAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        if (existing is null) return false;

        await _repository.DeleteAsync(existing, cancellationToken);

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
}