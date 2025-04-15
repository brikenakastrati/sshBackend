using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models.DTOs
{
    public class MenuOrderDTO
    {
        [Key]
        public int MenuOrderId { get; set; }

        public string OrderName { get; set; }

        public decimal OrderPrice { get; set; }

        public double AgencyFee { get; set; }

        public string Allergents { get; set; }

        public string IngreedientsForbiddenByReligion { get; set; }

        public string AdditionalRequests { get; set; }

        public int? EventId { get; set; }

        public int? OrderStatusId { get; set; }

      
        public string TenantId { get; set; }

    }
}
