
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;


public partial class MusicProvider
{
    [Key]
    public int MusicProviderId { get; set; }

    
    public string Name { get; set; }

   
    public string Address { get; set; }

  
    public string PhoneNumber { get; set; }

    
    public string Email { get; set; }

    
    public decimal? AgencyFee { get; set; }


    public int? PerformerTypeId { get; set; }

  
    public int? PartnerStatusId { get; set; }

  
    public decimal? BaseHourlyRate { get; set; }


    public virtual PartnerStatus PartnerStatus { get; set; }

    public virtual PerformerType PerformerType { get; set; }

   
    public virtual ICollection<PlaylistItem> PlaylistItems { get; set; } = new List<PlaylistItem>();

    // Fusha për multi-tenancy
    public string TenantId { get; set; }
}