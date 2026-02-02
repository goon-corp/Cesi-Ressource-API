using System;
using System.Collections.Generic;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.Logins.Models;

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
