using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.QuizzQuestions.Dtos;
using Ressource_API.Features.QuizzQuestions.Extensions;
using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.QuizzQuestions.Query;

namespace Ressource_API.Features.QuizzQuestions.Repositories;

public class QuizzQuestionRepository : BaseRepository<QuizzQuestion>, IQuizzQuestionRepository
{
    public QuizzQuestionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PaginatedList<QuizzQuestionInfoDto>> PaginatedQuizzQuestionsAsync(
        QuizzQuestionQuery query,
        CancellationToken cancellationToken = default)
    {
        var questions = _context.QuizzesQuestions
            .AsQueryable()
            .OrderByDescending(q => q.CreationTime)
            .Where(q => q.DeletionTime == null);

        if (query.QuizzId.HasValue)
            questions = questions.Where(q => q.QuizzId == query.QuizzId.Value);

        if (query.CreatedAt.HasValue)
        {
            var date = query.CreatedAt.Value.ToDateTime(TimeOnly.MinValue);
            questions = questions.Where(q => q.CreationTime.Date == date);
        }

        if (query.IsDeleted is not null)
        {
            questions = (bool)query.IsDeleted
                ? questions.Where(q => q.DeletionTime != null)
                : questions.Where(q => q.DeletionTime == null);
        }

        var totalCount = await questions.CountAsync(cancellationToken);

        var entities = await questions
            .Include(q => q.Quizz)
            .Include(q => q.Users)
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<QuizzQuestionInfoDto>(
            entities.Select(q => q.ToInfoDto()).ToList(),
            query.page, query.size, totalCount);
    }

    public async Task<QuizzQuestion?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.QuizzesQuestions
            .Include(q => q.Quizz)
            .Include(q => q.Users)
            .FirstOrDefaultAsync(q => q.Id == id && q.DeletionTime == null, cancellationToken);
    }
}