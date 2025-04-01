using sshBackend1.Models;
using sshBackend1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;

public class Event 
{
    [Key]
    public int EventId { get; set; }

    public string EventName { get; set; }

    public int? EventTypeId { get; set; }

    public DateTime? EventDate { get; set; }

    public string TenantId { get; set; }
    public virtual ICollection<FlowerArrangementOrder> FlowerArrangementOrders { get; set; } = new List<FlowerArrangementOrder>();

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();

    public virtual ICollection<MenuOrder> MenuOrders { get; set; } = new List<MenuOrder>();

    public virtual ICollection<MusicProviderOrder> MusicProviderOrders { get; set; } = new List<MusicProviderOrder>();

    public virtual ICollection<PastryOrder> PastryOrders { get; set; } = new List<PastryOrder>();

    public virtual ICollection<VenueOrder> VenueOrders { get; set; } = new List<VenueOrder>();
}
