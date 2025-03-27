
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;

public partial class PastryType
{
    [Key]
    public int PastryTypeId { get; set; }

    public string TypeName { get; set; }

    public virtual ICollection<Pastry> Pastries { get; set; } = new List<Pastry>();

    // Fusha për multi-tenancy
    public string TenantId { get; set; }
}
