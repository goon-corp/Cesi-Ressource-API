using System;
using System.Collections.Generic;
using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.Ressources.Models;

namespace Ressource_API.Features.Quizzes.Models;

public partial class Quizz
{
    public Guid Id { get; set; }

    public long ParticipationCount { get; set; }

    public Guid RessourceId { get; set; }

    public virtual ICollection<QuizzQuestion> QuizzesQuestions { get; set; } = new List<QuizzQuestion>();

    public virtual Ressource Ressource { get; set; } = null!;
}
