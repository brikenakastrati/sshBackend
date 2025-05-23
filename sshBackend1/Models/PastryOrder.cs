
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models;

public partial class PastryOrder
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int PastryOrderId { get; set; }
    public int Id => PastryOrderId;

    public string Name { get; set; }

    public decimal? OrderPrice { get; set; }

    public double? AgencyFee { get; set; }


    public int EventId { get; set; }

    public int OrderStatusId { get; set; }

    public virtual Event Event { get; set; }

    public virtual OrderStatus OrderStatus { get; set; }
    public virtual PastryShop OrderShop { get; set; }
    public int ShopId { get; set; }


}