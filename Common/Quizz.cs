using System;
using System.Collections.Generic;

namespace Features;

public partial class Quizz
{
    public Guid Id { get; set; }

    public long ParticipationCount { get; set; }

    public Guid RessourceId { get; set; }

    public virtual ICollection<QuizzQuestion> QuizzesQuestions { get; set; } = new List<QuizzQuestion>();

    public virtual Ressource Ressource { get; set; } = null!;
}
