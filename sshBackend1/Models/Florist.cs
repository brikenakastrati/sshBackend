﻿using sshBackend1.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;

public partial class Florist 
{
    [Key]
    public int FloristId { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public decimal? AgencyFee { get; set; }

    public int? PartnerStatusId { get; set; }

  

    public virtual ICollection<FlowerArrangement> FlowerArrangements { get; set; } = new List<FlowerArrangement>();

    public virtual PartnerStatus PartnerStatus { get; set; }
}
