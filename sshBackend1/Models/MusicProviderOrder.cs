
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;

public partial class MusicProviderOrder
{
    [Key]
    public int MusicProviderOrderId { get; set; }

    public string OrderName { get; set; }

    public decimal OrderPrice { get; set; }

    public double AgencyFee { get; set; }

    public string Notes { get; set; }

    public string MusicProviderAddress { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public int? EventId { get; set; }

    public int? OrderStatusId { get; set; }

    public virtual Event Event { get; set; }

    public virtual OrderStatus OrderStatus { get; set; }

    // Fusha për multi-tenancy
    public string TenantId { get; set; }
}