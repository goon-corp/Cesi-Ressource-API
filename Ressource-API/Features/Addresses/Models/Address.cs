using System;
using System.Collections.Generic;
using Ressource_API.Features.Cities.Models;
using Ressource_API.Features.Departments.Models;
using Ressource_API.Features.Regions.Models;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.Addresses.Models;

public partial class Address
{
    public Guid Id { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public DateTime CreationTime { get; set; }

    public int RegionId { get; set; }

    public int DepartmentId { get; set; }

    public int CityId { get; set; }

    public Guid UserId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual Region Region { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
