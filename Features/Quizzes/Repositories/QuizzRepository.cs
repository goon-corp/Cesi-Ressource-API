using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Quizzes.Dtos;
using Ressource_API.Features.Quizzes.Extensions;
using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.Quizzes.Query;

namespace Ressource_API.Features.Quizzes.Repositories;

public class QuizzRepository : BaseRepository<Quizz>, IQuizzRepository
{
    public QuizzRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PaginatedList<QuizzInfoDto>> PaginatedQuizzesAsync(
        QuizzQuery query,
        CancellationToken cancellationToken = default)
    {
        var quizzes = _context.Quizzes.AsQueryable();

        if (query.RessourceId.HasValue)
            quizzes = quizzes.Where(q => q.RessourceId == query.RessourceId.Value);

        var totalCount = await quizzes.CountAsync(cancellationToken);

        var entities = await quizzes
            .Include(q => q.Ressource)
            .Include(q => q.QuizzesQuestions)
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<QuizzInfoDto>(
            entities.Select(q => q.ToInfoDto()).ToList(),
            query.page, query.size, totalCount);
    }

    public async Task<Quizz?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Quizzes
            .Include(q => q.Ressource)
            .Include(q => q.QuizzesQuestions)
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }
}