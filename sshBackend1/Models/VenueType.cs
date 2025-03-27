using System;
using System.Collections.Generic;

namespace sshBackend1.Models;

public partial class VenueType
{
    public int VenueTypeId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Venue> Venues { get; set; } = new List<Venue>();

    // Fusha për multi-tenancy
    public string TenantId { get; set; }
}