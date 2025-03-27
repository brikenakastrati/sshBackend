
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;

public partial class PartnerStatus
{
    [Key]
    public int PartnerStatusId { get; set; }

    public string Name { get; set; }


    public virtual ICollection<Florist> Florists { get; set; } = new List<Florist>();



    public virtual ICollection<MusicProvider> MusicProviders { get; set; } = new List<MusicProvider>();

    public virtual ICollection<PastryShop> PastryShops { get; set; } = new List<PastryShop>();

    public virtual ICollection<VenueProvider> VenueProviders { get; set; } = new List<VenueProvider>();

    // Fusha për multi-tenancy
    public string TenantId { get; set; }
}