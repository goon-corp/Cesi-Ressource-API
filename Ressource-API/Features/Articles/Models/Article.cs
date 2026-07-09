using System;
using System.Collections.Generic;
using Ressource_API.Features.Ressources.Models;

namespace Ressource_API.Features.Articles.Models;

public partial class Article
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public Guid RessourceId { get; set; }

    public virtual Ressource Ressource { get; set; } = null!;
}
