using System;
using System.Collections.Generic;

namespace Features;

public partial class ProfilePicture
{
    public Guid Id { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public DateTime CreationTime { get; set; }

    public string ImageUrl { get; set; } = null!;

    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
