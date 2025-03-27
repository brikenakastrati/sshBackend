
using sshBackend1.Models;
using System;
using System.Collections.Generic;

namespace sshBackend1.Models;

public partial class FlowerArrangementOrder
{
    public int FlowerArrangementOrderId { get; set; }

    public string OrderName { get; set; }

    public decimal OrderPrice { get; set; }

    public double AgencyFee { get; set; }

    public string OrderDescription { get; set; }

    public string Notes { get; set; }

    public int? EventId { get; set; }

    public int? OrderStatusId { get; set; }

    public virtual Event Event { get; set; }
    public virtual OrderStatus OrderStatus { get; set; }

    // Fusha për multi-tenancy
    public string TenantId { get; set; }

}
