
using sshBackend1.Models;
using System;
using System.Collections.Generic;

namespace sshBackend1.Models;

public partial class GuestStatus
{
    public int GuestStatusId { get; set; }

    public string GuestStatusName { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();
}