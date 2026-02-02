using System;
using System.Collections.Generic;

namespace Features;

public partial class Poll
{
    public Guid Id { get; set; }

    public long VoteCount { get; set; }

    public Guid RessourceId { get; set; }

    public virtual ICollection<PollOption> PollsOptions { get; set; } = new List<PollOption>();

    public virtual Ressource Ressource { get; set; } = null!;
}
