using System;
using System.Collections.Generic;
using Ressource_API.Features.Addresses.Models;

namespace Ressource_API.Features.Cities.Models;

public partial class City
{
    public int Id { get; set; }

    public string DepartmentCode { get; set; } = null!;

    public string? InseeCode { get; set; }

    public string? ZipCode { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public float GpsLat { get; set; }

    public float GpsLng { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
