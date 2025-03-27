
using sshBackend1.Models;
using System;
using System.Collections.Generic;

namespace sshBackend1.Models;

public partial class VenueProvider
{
    public int VenueProviderId { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public decimal? AgencyFee { get; set; }

    public int? PartnerStatusId { get; set; }

    public virtual PartnerStatus PartnerStatus { get; set; }

    public virtual ICollection<Venue> Venues { get; set; } = new List<Venue>();

    // Fusha për multi-tenancy
    public string TenantId { get; set; }
}