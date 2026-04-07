using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.QuestionAnswers.Dtos;
using Ressource_API.Features.QuizzAnswer.Extensions;
using Ressource_API.Features.QuizzAnswer.Models;
using Ressource_API.Features.QuizzAnswer.Query;

namespace Ressource_API.Features.QuizzAnswer.Repositories;

public class QuestionAnswerRepository : BaseRepository<QuestionAnswer>, IQuestionAnswerRepository
{
    public QuestionAnswerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PaginatedList<QuestionAnswerInfoDto>> PaginatedQuestionAnswersAsync(
        QuestionAnswerQuery query,
        CancellationToken cancellationToken = default)
    {
        var answers = _context.QuestionAnswers.AsQueryable();

        if (query.UserId.HasValue)
            answers = answers.Where(a => a.UserId == query.UserId.Value);

        if (query.QuizzQuestionId.HasValue)
            answers = answers.Where(a => a.QuizzQuestionId == query.QuizzQuestionId.Value);

        var totalCount = await answers.CountAsync(cancellationToken);

        var entities = await answers
            .Include(a => a.User)
            .Include(a => a.QuizzQuestion)
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<QuestionAnswerInfoDto>(
            entities.Select(a => a.ToInfoDto()).ToList(),
            query.page, query.size, totalCount);
    }

    public async Task<QuestionAnswer?> FindByUsersAndQuestionAsync(
        Guid userId,
        Guid quizzQuestionId,
        CancellationToken cancellationToken = default)
    {
        return await _context.QuestionAnswers
            .Include(a => a.User)
            .Include(a => a.QuizzQuestion)
            .FirstOrDefaultAsync(
                a => a.UserId == userId && a.QuizzQuestionId == quizzQuestionId,
                cancellationToken);
    }
}