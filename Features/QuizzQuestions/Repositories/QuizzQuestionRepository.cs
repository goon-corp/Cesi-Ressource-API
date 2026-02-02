using Ressource_API.Common.Data;
using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.QuizzQuestions.Repositories;

public class QuizzQuestionRepository : BaseRepository<QuizzQuestion>, IQuizzQuestionRepository
{
    public QuizzQuestionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
