namespace sshBackend1.Models.DTOs
{
    public class TableDTO
    {
        public int TableId { get; set; }
        public int NumberOfSeats { get; set; }
        public int? TableStatusId { get; set; }
        public int? EventId { get; set; }


        public string TenantId { get; set; }
    }
}
