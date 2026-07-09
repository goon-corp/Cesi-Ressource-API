using System;
using System.Collections.Generic;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.Notifications.Models;

public partial class Notification
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public bool MarkedAsRead { get; set; }

    public string Content { get; set; } = null!;

    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
