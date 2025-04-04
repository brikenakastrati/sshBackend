using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
    public class OrderStatusDTO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int OrderStatusId { get; set; }

        public string OrderStatusName { get; set; }

        public string TenantId { get; set; }

    }
}
