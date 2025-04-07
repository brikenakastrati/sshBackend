using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
    public class VenueDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int VenueId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public string Address { get; set; }

        public int? VenueProviderId { get; set; }

        public int? VenueTypeId { get; set; }

   
        public string TenantId { get; set; }
    }
}
