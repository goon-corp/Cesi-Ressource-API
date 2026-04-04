using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.Users.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Extensions;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;
using Ressource_API.Features.Tags.TagDtos;
using Ressource_API.Features.Users.Dtos;
using Ressource_API.Features.Users.Extensions;
using Ressource_API.Features.Users.Query;
using Ressource_API.Features.Users.UserDtos;

namespace Ressource_API.Features.Users.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> FindWithUserRoleAsync(Guid userId)
    {
        return await _context.Set<User>()
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Join(
                _context.Logins,
                u => u.Id,
                l => l.UserId,
                (u, l) => new UserProfileDto()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    Email = l.Email,
                    AuthoredRessourcesCount = u.AuthoredRessources.Count(),
                    LikedRessourcesCount = u.LikedRessources.Count(),
                    FavoriteRessourcesCount = u.FavoritedRessources.Count(),
                })
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("cannot provide user profile");
    }

    public async Task<PaginatedList<ReturnRessourceDto>> GetUserAuthoredRessourcesAsync(Guid userId,
        PagedQueryParameters query, CancellationToken cancellationToken = default)
    {
        var ressources = _context.Ressources
            .Where(r => r.UserId == userId && r.DeletionTime == null)
            .OrderByDescending(r => r.CreationTime);

        var totalCount = await ressources.CountAsync(cancellationToken);

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
                ConfidentialityType = new RessourceConfidentialityTypeInfoDto
                    { Id = r.RessourceConfidentialityType.Id, Label = r.RessourceConfidentialityType.Label },
                Type = new RessourceTypeInfoDto { Id = r.RessourceType.Id, Label = r.RessourceType.Label },
                Tags = r.Tags.Select(t => new ReturnTagDto { Id = t.Id, Label = t.Label }),
                LikeCount = r.LikedByUsers.Count(),
                FavoriteCount = r.FavoritedByUsers.Count()
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<ReturnRessourceDto>(dtos, query.page, query.size, totalCount);
    }

    public async Task<PaginatedList<ReturnRessourceDto>> GetUserLikedRessourcesAsync(Guid userId, PagedQueryParameters query,
        CancellationToken cancellationToken = default)
    {
        var ressources = _context.Ressources
            .Where(r => r.LikedByUsers.Any(u => u.Id == userId) && r.DeletionTime == null)
            .OrderByDescending(r => r.CreationTime);

        var totalCount = await ressources.CountAsync(cancellationToken);

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
                ConfidentialityType = new RessourceConfidentialityTypeInfoDto
                    { Id = r.RessourceConfidentialityType.Id, Label = r.RessourceConfidentialityType.Label },
                Type = new RessourceTypeInfoDto { Id = r.RessourceType.Id, Label = r.RessourceType.Label },
                Tags = r.Tags.Select(t => new ReturnTagDto { Id = t.Id, Label = t.Label }),
                LikeCount = r.LikedByUsers.Count(),
                FavoriteCount = r.FavoritedByUsers.Count()
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<ReturnRessourceDto>(dtos, query.page, query.size, totalCount);
    }

    public async Task<PaginatedList<ReturnRessourceDto>> GetUserFavoriteRessourcesAsync(Guid userId,
        PagedQueryParameters query, CancellationToken cancellationToken = default)
    {
        var ressources = _context.Ressources
            .Where(r => r.FavoritedByUsers.Any(u => u.Id == userId) && r.DeletionTime == null)
            .OrderByDescending(r => r.CreationTime);

        var totalCount = await ressources.CountAsync(cancellationToken);

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
                ConfidentialityType = new RessourceConfidentialityTypeInfoDto
                    { Id = r.RessourceConfidentialityType.Id, Label = r.RessourceConfidentialityType.Label },
                Type = new RessourceTypeInfoDto { Id = r.RessourceType.Id, Label = r.RessourceType.Label },
                Tags = r.Tags.Select(t => new ReturnTagDto { Id = t.Id, Label = t.Label }),
                LikeCount = r.LikedByUsers.Count(),
                FavoriteCount = r.FavoritedByUsers.Count()
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<ReturnRessourceDto>(dtos, query.page, query.size, totalCount);
    }
    public async Task<PaginatedList<UserInfoDto>> PaginatedUsersAsync(
         UserQuery query,
        CancellationToken cancellationToken = default)
    {
        var users = _context.Users
            .AsQueryable()
            .OrderByDescending(u => u.CreationTime)
            .Where(u => u.DeletionTime == null);

        if (!string.IsNullOrWhiteSpace(query.UserName))
            users = users.Where(u => u.UserName.Contains(query.UserName));

        if (!string.IsNullOrWhiteSpace(query.FirstName))
            users = users.Where(u => u.FirstName.Contains(query.FirstName));

        if (!string.IsNullOrWhiteSpace(query.LastName))
            users = users.Where(u => u.LastName.Contains(query.LastName));

        if (query.UserRoleId.HasValue)
            users = users.Where(u => u.UserRoleId == query.UserRoleId.Value);

        if (query.IsActive.HasValue)
            users = users.Where(u => u.IsActive == query.IsActive.Value);

        if (query.CreatedAt.HasValue)
        {
            var date = query.CreatedAt.Value.ToDateTime(TimeOnly.MinValue);
            users = users.Where(u => u.CreationTime.Date == date);
        }

        if (query.IsDeleted is not null)
        {
            users = (bool)query.IsDeleted
                ? users.Where(u => u.DeletionTime != null)
                : users.Where(u => u.DeletionTime == null);
        }

        var totalCount = await users.CountAsync(cancellationToken);

        var entities = await users
            .Include(u => u.UserRole)
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<UserInfoDto>(
            entities.Select(u => u.ToInfoDto()).ToList(),
            query.page, query.size, totalCount);
    }

    public async Task<User?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.Id == id && u.DeletionTime == null, cancellationToken);
    }

    public async Task<User?> FindByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.UserName == userName && u.DeletionTime == null, cancellationToken);
    }
}
