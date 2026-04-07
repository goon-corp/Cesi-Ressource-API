using System;
using System.Collections.Generic;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.ProfilePictures.Models;

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
