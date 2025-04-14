using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
    public class RestaurantDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RestaurantId { get; set; } 
        public string RestaurantName { get; set; } 
        public string Address { get; set; } 
        public string PhoneNumber { get; set; } 
        public int? RestaurantStatusId { get; set; } 

 
        public string TenantId { get; set; }

       
        public int VenueId { get; set; } 

       
    }
}
