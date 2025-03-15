
using sshBackend1.Models;
using System;
using System.Collections.Generic;

namespace RPPP_WebApp.Models;

public partial class VenueType
{
    public int VenueTypeId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Venue> Venues { get; set; } = new List<Venue>();
}