using Ressource_API.Common.Data;
using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Quizzes.Repositories;

public class QuizzRepository : BaseRepository<Quizz>, IQuizzRepository
{
    public QuizzRepository(ApplicationDbContext context) : base(context)
    {
    }
}
