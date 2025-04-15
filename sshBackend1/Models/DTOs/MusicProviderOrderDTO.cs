using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
    public class MusicProviderOrderDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MusicProviderOrderId { get; set; }

        public string OrderName { get; set; }

        public decimal OrderPrice { get; set; }

        public double AgencyFee { get; set; }

        public string Notes { get; set; }

        public string MusicProviderAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int? EventId { get; set; }

        public int? OrderStatusId { get; set; }

        public string TenantId { get; set; }
    }
}
