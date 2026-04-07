using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.Quizzes.Query;

public class QuizzQuery : PagedQueryParameters
{
    public Guid? RessourceId { get; set; }
}