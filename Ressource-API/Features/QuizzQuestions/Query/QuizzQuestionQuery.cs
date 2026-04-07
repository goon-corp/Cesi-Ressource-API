using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.QuizzQuestions.Query;

public class QuizzQuestionQuery : PagedQueryParameters
{
    public Guid? QuizzId { get; set; }
    public bool? IsDeleted { get; set; }
    public DateOnly? CreatedAt { get; set; }
}