using System;
using System.Collections.Generic;

namespace Features;

public partial class RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
