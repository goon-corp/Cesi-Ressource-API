using System;
using System.Collections.Generic;
using Ressource_API.Features.Ressources.Models;

namespace Ressource_API.Features.RessourceMedias.Models;

public partial class RessourceMedia
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string MediaUrl { get; set; } = null!;

    public string MimeType { get; set; } = null!;
}
