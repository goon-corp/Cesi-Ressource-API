using System;
using System.Collections.Generic;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.RefreshTokens.Models;

public partial class RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = null!;

    public bool IsActive { get; set; }
    
    public DateTime ExpirationTime { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
