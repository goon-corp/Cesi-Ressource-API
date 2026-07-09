using System;
using System.Collections.Generic;
using Ressource_API.Features.BackofficeLogLevels.Models;
using Ressource_API.Features.BackofficeOperationTypes.Models;

namespace Ressource_API.Features.BackofficeLogs.Models;

public partial class BackofficeLog
{
    public Guid Id { get; set; }

    public DateTime EventTime { get; set; }

    public string LogContent { get; set; } = null!;

    public Guid BackofficeLogLevelId { get; set; }

    public Guid BackofficeOperationTypeId { get; set; }

    public virtual BackofficeLogLevel BackofficeLogLevel { get; set; } = null!;

    public virtual BackofficeOperationType BackofficeOperationType { get; set; } = null!;
}
