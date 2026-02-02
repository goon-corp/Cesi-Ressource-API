using System;
using System.Collections.Generic;

namespace Features;

public partial class ReportType
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string Label { get; set; } = null!;

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
