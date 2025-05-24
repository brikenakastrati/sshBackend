using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sshBackend1.Models;

namespace sshBackend1.Models;

public partial class VenueOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VenueOrderId { get; set; }

    public int VenueId { get; set; }
    public int Id => VenueOrderId;

    public string Name { get; set; }

    public string? Description { get; set; }


    public int EventId { get; set; }

    public int OrderStatusId { get; set; }

    public virtual Event Event { get; set; }

    public virtual OrderStatus OrderStatus { get; set; }

    public virtual VenueProvider VenueProvider { get; set; }
    public int VenueProviderId { get; set; }


}