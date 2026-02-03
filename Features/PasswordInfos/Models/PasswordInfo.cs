using System;
using System.Collections.Generic;
using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.PasswordInfos.Models;

public partial class PasswordInfo
{
    public Guid Id { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public DateTime CreationTime { get; set; }

    public int AttemptCount { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetDate { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<PasswordHistory> PasswordsHistories { get; set; } = new List<PasswordHistory>();

    public virtual User User { get; set; } = null!;
}
