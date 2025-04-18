using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
    public class PlaylistItemDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlaylistItemId { get; set; }


        public string Name { get; set; }

        public int GenreId { get; set; }


        public int MusicProviderId { get; set; }


        public string Length { get; set; }

        public string TenantId { get; set; }
    }
}
