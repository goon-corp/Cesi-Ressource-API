using System;
using System.Collections.Generic;
using Ressource_API.Features.BackofficeLogs.Models;

namespace Ressource_API.Features.BackofficeOperationTypes.Models;

public partial class BackofficeOperationType
{
    public Guid Id { get; set; }

    public string Label { get; set; } = null!;

    public virtual ICollection<BackofficeLog> BackofficeLogs { get; set; } = new List<BackofficeLog>();
}
