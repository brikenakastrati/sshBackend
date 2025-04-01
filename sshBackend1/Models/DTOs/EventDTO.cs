using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
    public class EventDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        public string EventName { get; set; }

        public int? EventTypeId { get; set; }

        public DateTime? EventDate { get; set; }

        public string TenantId { get; set; }
    }
}
