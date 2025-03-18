
using sshBackend1.Models;
using System;
using System.Collections.Generic;

namespace sshBackend1.Models;

public partial class MenuType
{
    public int MenuTypeId { get; set; }

    public string TypeName { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();
}
