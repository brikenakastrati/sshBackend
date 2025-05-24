
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models;

public partial class FlowerArrangementOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FlowerArrangementOrderId { get; set; }

    public string Name { get; set; }
    public int Id => FlowerArrangementOrderId;

    public decimal OrderPrice { get; set; }

    public double AgencyFee { get; set; }

    public string OrderDescription { get; set; }

    public string Notes { get; set; }

    public int EventId { get; set; }

    public int OrderStatusId { get; set; }

    public virtual Event Event { get; set; }
    public virtual OrderStatus OrderStatus { get; set; }
    public virtual Florist Florist { get; set; }
    public int FloristId { get; set; }
}
