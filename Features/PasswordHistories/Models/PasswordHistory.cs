using System;
using System.Collections.Generic;
using Ressource_API.Features.PasswordInfos.Models;

namespace Ressource_API.Features.PasswordHistories.Models;

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
