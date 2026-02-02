using System;
using System.Collections.Generic;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.SessionMessages.Models;

namespace Ressource_API.Features.Sessions.Models;

public partial class Session
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string IdWs { get; set; } = null!;

    public string Status { get; set; } = null!;

    public Guid RessourceId { get; set; }

    public virtual Ressource Ressource { get; set; } = null!;

    public virtual ICollection<SessionMessage> SessionsMessages { get; set; } = new List<SessionMessage>();
}
