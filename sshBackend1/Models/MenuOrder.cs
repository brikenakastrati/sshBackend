
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;

public partial class MenuOrder
{
    [Key]
    public int MenuOrderId { get; set; }

    public string OrderName { get; set; }

    public decimal OrderPrice { get; set; }

    public double AgencyFee { get; set; }

    public string Allergents { get; set; }

    public string IngreedientsForbiddenByReligion { get; set; }

    public string AdditionalRequests { get; set; }

    public int? EventId { get; set; }

    public int? OrderStatusId { get; set; }

    public virtual Event Event { get; set; }
    public virtual OrderStatus OrderStatus { get; set; }

    // Fusha për multi-tenancy
    public string TenantId { get; set; }

}
