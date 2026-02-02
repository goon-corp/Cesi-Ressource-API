using System;
using System.Collections.Generic;

namespace Features;

public partial class UserRole
{
    public Guid Id { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public DateTime CreationTime { get; set; }

    public string RoleLabel { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
