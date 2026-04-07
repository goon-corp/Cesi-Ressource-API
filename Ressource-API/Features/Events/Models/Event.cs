using System;
using System.Collections.Generic;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.Events.Models;

public class Event
{
    public Guid Id { get; set; }

    public bool IsVirtual { get; set; }

    public DateTime DateStart { get; set; }

    public DateTime DateEnd { get; set; }

    public string? EventLink { get; set; }

    public string Location { get; set; } = null!;

    public Guid RessourceId { get; set; }

    public virtual Ressource Ressource { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
