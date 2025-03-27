
using sshBackend1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models;


public partial class PlaylistItem
{
    [Key]
    public int PlaylistItemId { get; set; }

  
    public string Name { get; set; }

    public int GenreId { get; set; }

   
    public int MusicProviderId { get; set; }

  
    public string Length { get; set; }

  
 

    public virtual MusicProvider MusicProvider { get; set; }

    // Fusha për multi-tenancy
    public string TenantId { get; set; }
}