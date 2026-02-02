using System;
using System.Collections.Generic;
using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.QuizzQuestions.Models;

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
