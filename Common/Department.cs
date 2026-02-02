using System;
using System.Collections.Generic;

namespace Features;

public partial class Department
{
    public int Id { get; set; }

    public string RegionCode { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
