using System;
using System.Collections.Generic;
using Ressource_API.Features.ReportTypes.Models;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.Reports.Models;

public partial class Report
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public bool IsCheckedByModerator { get; set; }

    public Guid ReportTypeId { get; set; }

    public Guid UserId { get; set; }

    public Guid RessourceId { get; set; }

    public virtual ReportType ReportType { get; set; } = null!;

    public virtual Ressource Ressource { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
