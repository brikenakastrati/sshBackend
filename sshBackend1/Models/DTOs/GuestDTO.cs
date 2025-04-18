using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models.DTOs
{
    public class GuestDTO
    {
        [Key]
        public int GuestId { get; set; }

        public string GuestName { get; set; }

        public string GuestSecondName { get; set; }

        public string GuestSurname { get; set; }

        public int GuestStatusId { get; set; }

        public int EventId { get; set; }

        public int TableId { get; set; }

        public string TenantId { get; set; }
    }
}
