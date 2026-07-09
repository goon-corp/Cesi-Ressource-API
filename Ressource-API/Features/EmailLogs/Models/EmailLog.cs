using System;
using System.Collections.Generic;

namespace Ressource_API.Features.EmailLogs.Models;

public partial class EmailLog
{
    public Guid Id { get; set; }

    public DateTime SentTime { get; set; }

    public string Content { get; set; } = null!;

    public string SenderEmail { get; set; } = null!;

    public string ReceiverEmail { get; set; } = null!;

    public string OperationType { get; set; } = null!;
}
