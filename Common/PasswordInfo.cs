using System;
using System.Collections.Generic;

namespace Features;

public partial class PasswordInfo
{
    public Guid Id { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public DateTime CreationTime { get; set; }

    public int AttemptCount { get; set; }

    public Guid? ResetToken { get; set; }

    public DateTime? ResetDate { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<PasswordHistory> PasswordsHistories { get; set; } = new List<PasswordHistory>();

    public virtual User User { get; set; } = null!;
}
