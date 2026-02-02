using System;
using System.Collections.Generic;

namespace Features;

public partial class RessourceProgression
{
    public Guid RessourceId { get; set; }

    public Guid UserId { get; set; }

    public bool IsAside { get; set; }

    public bool IsExploited { get; set; }

    public virtual Ressource Ressource { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
