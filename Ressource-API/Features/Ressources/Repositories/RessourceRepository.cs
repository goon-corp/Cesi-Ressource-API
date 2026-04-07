using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Query;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;
using Ressource_API.Features.Tags.TagDtos;

namespace Ressource_API.Features.Ressources.Repositories;

public class RessourceRepository : BaseRepository<Ressource>, IRessourceRepository
{
    public RessourceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Ressource?> FindWithTagsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Ressources
            .Include(r => r.Tags)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool?> ToggleLikeAsync(Guid ressourceId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        // Single query: check existence + whether user already liked it
        var projection = await _context.Ressources
            .Where(r => r.Id == ressourceId)
            .Select(r => new { AlreadyLiked = r.LikedByUsers.Any(u => u.Id == userId) })
            .FirstOrDefaultAsync(cancellationToken);

        if (projection is null) return null;

        if (projection.AlreadyLiked)
        {
            await _context.Database.ExecuteSqlAsync(
                $"DELETE FROM ressource_like WHERE ressource_id = {ressourceId} AND user_id = {userId}",
                cancellationToken);
            return false;
        }

        var userExists = await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
        if (!userExists) return null;

        await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO ressource_like (ressource_id, user_id) VALUES ({ressourceId}, {userId})",
            cancellationToken);
        return true;
    }

    public async Task<bool?> ToggleFavoriteAsync(Guid ressourceId, Guid userId,
        CancellationToken cancellationToken = default)
    {
        var projection = await _context.Ressources
            .Where(r => r.Id == ressourceId)
            .Select(r => new { AlreadyFavorited = r.FavoritedByUsers.Any(u => u.Id == userId) })
            .FirstOrDefaultAsync(cancellationToken);

        if (projection is null) return null;

        if (projection.AlreadyFavorited)
        {
            await _context.Database.ExecuteSqlAsync(
                $"DELETE FROM ressource_favorite WHERE ressource_id = {ressourceId} AND user_id = {userId}",
                cancellationToken);
            return false;
        }

        var userExists = await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
        if (!userExists) return null;

        await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO ressource_favorite (ressource_id, user_id) VALUES ({ressourceId}, {userId})",
            cancellationToken);
        return true;
    }

    public async Task<PaginatedList<ReturnRessourceDto>> PaginatedRessourcesAsync(RessourceQuery query,
        CancellationToken cancellationToken = default)
    {
        var ressources = _context.Ressources.AsQueryable();

        // Filtre SQL:

        ressources = ressources.OrderByDescending(r => r.CreationTime)
            .Where(r => r.DeletionTime == null);

        if (!string.IsNullOrWhiteSpace(query.RessourceTitle))
        {
            ressources = ressources.Where(r => r.Title.Contains(query.RessourceTitle));
        }

        // filtrage par tag
        if (query.RessourceTags is { Count: > 0 })
        {
            ressources = ressources.Where(r => r.Tags.Any(t => query.RessourceTags.Contains(t.Id)));
        }

        if (!string.IsNullOrWhiteSpace(query.RessourceType))
        {
            ressources = ressources.Where(r => r.RessourceType.Label.Contains(query.RessourceType));
        }

        if (query.CreatedAt.HasValue)
        {
            var date = query.CreatedAt.Value;
            ressources = ressources.Where(r => r.CreationTime.Date == date);
        }

        if (query.IsDeleted is not null)
        {
            ressources = (bool)query.IsDeleted
                ? ressources.Where(t => t.DeletionTime != null)
                : ressources.Where(t => t.DeletionTime == null);
        }

        var totalCount = await ressources.CountAsync(cancellationToken);

        // -------------------------
        // Pagination SQL
        // -------------------------
        var dtos = await ressources
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .Select(r => new ReturnRessourceDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                ThumbnailId = r.ThumbnailId,
                Status = new RessourceStatusInfoDto { Id = r.RessourceStatus.Id, Label = r.RessourceStatus.Label },
                ConfidentialityType = new RessourceConfidentialityTypeInfoDto { Id = r.RessourceConfidentialityType.Id, Label = r.RessourceConfidentialityType.Label },
                Type = new RessourceTypeInfoDto { Id = r.RessourceType.Id, Label = r.RessourceType.Label },
                Tags = r.Tags.Select(t => new ReturnTagDto { Id = t.Id, Label = t.Label }),
                UserId = r.UserId,
                LikeCount = r.LikedByUsers.Count(),
                FavoriteCount = r.FavoritedByUsers.Count()
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<ReturnRessourceDto>(dtos, query.page, query.size, totalCount);
    }
}