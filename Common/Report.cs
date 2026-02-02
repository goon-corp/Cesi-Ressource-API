using System;
using System.Collections.Generic;

namespace Features;

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
