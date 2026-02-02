using System;
using System.Collections.Generic;

namespace Features;

public partial class BackofficeOperationType
{
    public Guid Id { get; set; }

    public string Label { get; set; } = null!;

    public virtual ICollection<BackofficeLog> BackofficeLogs { get; set; } = new List<BackofficeLog>();
}
