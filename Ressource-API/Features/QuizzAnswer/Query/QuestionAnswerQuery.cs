using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.QuizzAnswer.Query;

public class QuestionAnswerQuery : PagedQueryParameters
{
    public Guid? UserId { get; set; }
    public Guid? QuizzQuestionId { get; set; }
}