using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.QuizzAnswer.Models;

public partial class QuestionAnswer
{
    public string Answer { get; set; } = null!;

    public Guid UserId { get; set; }

    public Guid QuizzQuestionId { get; set; }

    public virtual QuizzQuestion QuizzQuestion { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
