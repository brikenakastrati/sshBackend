
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;

public partial class PerformerType
{
    [Key]
    public int PerformerTypeId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<MusicProvider> MusicProviders { get; set; } = new List<MusicProvider>();

    // Fusha për multi-tenancy
    public string TenantId { get; set; }
}
