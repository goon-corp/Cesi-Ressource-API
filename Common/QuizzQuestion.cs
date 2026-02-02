using System;
using System.Collections.Generic;

namespace Features;

public partial class QuizzQuestion
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string Question { get; set; } = null!;

    public string PossibleAnswers { get; set; } = null!;

    public string CorrectAnswer { get; set; } = null!;

    public Guid QuizzId { get; set; }

    public virtual Quizz Quizz { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
