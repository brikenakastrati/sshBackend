using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models.DTOs
{
    public class PastryShopDTO
    {
        [Key]
        public int ShopId { get; set; }

        public string ShopName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public int? PartnerStatusId { get; set; }

    
        public string TenantId { get; set; }
    }
}
