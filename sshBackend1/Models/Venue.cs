using System;
using System.Collections.Generic;
using sshBackend1.Models;

namespace sshBackend1.Models;

public partial class Venue
{
    public int VenueId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal? Price { get; set; }

    public string Address { get; set; }

    public int? VenueProviderId { get; set; }

    public int? VenueTypeId { get; set; }

  

    public virtual ICollection<VenueOrder> VenueOrders { get; set; } = new List<VenueOrder>();

    public virtual VenueProvider VenueProvider { get; set; }
   
    public virtual VenueType VenueType { get; set; }

  
}