﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;

public partial class FlowerArrangementType
{
    [Key]
    public int FlowerArrangementTypeId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<FlowerArrangement> FlowerArrangements { get; set; } = new List<FlowerArrangement>();


}