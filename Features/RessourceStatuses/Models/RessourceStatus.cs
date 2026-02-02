using System;
using System.Collections.Generic;
using Ressource_API.Features.Ressources.Models;

namespace Ressource_API.Features.RessourceStatuses.Models;

public partial class RessourceStatus
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string Label { get; set; } = null!;

    public virtual ICollection<Ressource> Ressources { get; set; } = new List<Ressource>();
}
