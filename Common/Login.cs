using System;
using System.Collections.Generic;

namespace Features;

public partial class Login
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
