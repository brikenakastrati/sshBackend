
using sshBackend1.Models;
using System;
using System.Collections.Generic;

namespace sshBackend1.Models;

public partial class PastryShop
{
    public int ShopId { get; set; }

    public string ShopName { get; set; }

    public string Address { get; set; }

    public string PhoneNumber { get; set; }

    public int? PartnerStatusId { get; set; }

    public virtual PartnerStatus PartnerStatus { get; set; }

    public virtual ICollection<Pastry> Pastries { get; set; } = new List<Pastry>();
}