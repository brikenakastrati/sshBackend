
using sshBackend1.Models;
using System;
using System.Collections.Generic;

namespace sshBackend1.Models;

public partial class PastryType
{
    public int PastryTypeId { get; set; }

    public string TypeName { get; set; }

    public virtual ICollection<Pastry> Pastries { get; set; } = new List<Pastry>();
}
