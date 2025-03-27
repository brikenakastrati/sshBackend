
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;

public partial class GuestStatus
{
    [Key]
    public int GuestStatusId { get; set; }

    public string GuestStatusName { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();

    // Fusha për multi-tenancy
    public string TenantId { get; set; }
}