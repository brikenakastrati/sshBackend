
using sshBackend1.Models;
using System;
using System.Collections.Generic;

namespace sshBackend1.Models;

public partial class Menu
{
    public int MenuId { get; set; }

    public string MenuName { get; set; }

    public decimal Price { get; set; }

    public int? CateringId { get; set; }

    public int? MenuTypeId { get; set; }



    public virtual MenuType MenuType { get; set; }
}