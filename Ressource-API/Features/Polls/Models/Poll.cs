using System;
using System.Collections.Generic;
using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.Ressources.Models;

namespace Ressource_API.Features.Polls.Models;

public partial class Poll
{
    public Guid Id { get; set; }

    public long VoteCount { get; set; }

    public Guid RessourceId { get; set; }

    public virtual ICollection<PollOption> PollsOptions { get; set; } = new List<PollOption>();

    public virtual Ressource Ressource { get; set; } = null!;
}
