
using sshBackend1.Models;
using System;
using System.Collections.Generic;

namespace sshBackend1.Models;

public partial class PerformerType
{
    public int PerformerTypeId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<MusicProvider> MusicProviders { get; set; } = new List<MusicProvider>();
}
