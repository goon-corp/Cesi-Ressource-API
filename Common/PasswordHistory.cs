using System;
using System.Collections.Generic;

namespace Features;

public partial class PasswordHistory
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public Guid PasswordInfosId { get; set; }

    public virtual PasswordInfo PasswordInfos { get; set; } = null!;
}
