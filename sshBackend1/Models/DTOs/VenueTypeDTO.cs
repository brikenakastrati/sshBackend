using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
    public class VenueTypeDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int VenueTypeId { get; set; }

        public string Name { get; set; }


        // Fusha për multi-tenancy
        public string TenantId { get; set; }
    }
}
