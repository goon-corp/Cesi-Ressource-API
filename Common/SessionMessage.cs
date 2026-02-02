using System;
using System.Collections.Generic;

namespace Features;

public partial class SessionMessage
{
    public Guid Id { get; set; }

    public DateTime SentTime { get; set; }

    public string Content { get; set; } = null!;

    public Guid UserId { get; set; }

    public Guid SessionId { get; set; }

    public virtual Session Session { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
