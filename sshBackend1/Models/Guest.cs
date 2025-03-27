using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;

public partial class Guest
{
    [Key]
    public int GuestId { get; set; }

    public string GuestName { get; set; }

    public string GuestSecondName { get; set; }

    public string GuestSurname { get; set; }

    public int? GuestStatusId { get; set; }

    public int? EventId { get; set; }

    public int? TableId { get; set; }

    public virtual Event Event { get; set; }

    public virtual GuestStatus GuestStatus { get; set; }

    public virtual Table Table { get; set; }

    // Fusha për multi-tenancy
    public string TenantId { get; set; }
}