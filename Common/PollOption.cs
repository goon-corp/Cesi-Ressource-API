using System;
using System.Collections.Generic;

namespace Features;

public partial class PollOption
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string Option { get; set; } = null!;

    public Guid PollId { get; set; }

    public virtual Poll Poll { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
