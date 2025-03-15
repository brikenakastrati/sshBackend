
using sshBackend1.Data.Models;
using sshBackend1.Models;
using System;
using System.Collections.Generic;

namespace sshBackend1.Models;

public partial class OrderStatus
{
    public int OrderStatusId { get; set; }

    public string OrderStatusName { get; set; }

    public virtual ICollection<FlowerArrangementOrder> FlowerArrangementOrders { get; set; } = new List<FlowerArrangementOrder>();

 

    public virtual ICollection<MenuOrder> MenuOrders { get; set; } = new List<MenuOrder>();

    public virtual ICollection<MusicProviderOrder> MusicProviderOrders { get; set; } = new List<MusicProviderOrder>();

    public virtual ICollection<PastryOrder> PastryOrders { get; set; } = new List<PastryOrder>();

    public virtual ICollection<VenueOrder> VenueOrders { get; set; } = new List<VenueOrder>();
}