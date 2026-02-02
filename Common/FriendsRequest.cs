using System;
using System.Collections.Generic;

namespace Features;

public partial class FriendsRequest
{
    public Guid UserSenderId { get; set; }

    public Guid UserReceiverId { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public DateTime CreationTime { get; set; }

    public string RequestStatus { get; set; } = null!;

    public virtual User UserReceiver { get; set; } = null!;

    public virtual User UserSender { get; set; } = null!;
}
