using System;
using System.Collections.Generic;

namespace Features;

public partial class Comment
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string Content { get; set; } = null!;

    public Guid? CommentId { get; set; }

    public Guid RessourceId { get; set; }

    public Guid UserId { get; set; }

    public virtual Comment? ParentComment { get; set; }

    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();

    public virtual Ressource Ressource { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
