namespace sshBackend1.Models.DTOs
{
    public class VenueOrderDTO
    {
        public int VenueOrderId { get; set; }

        public int VenueId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public string Address { get; set; }

        public decimal? AgencyFee { get; set; }

        public string Notes { get; set; }

        public int? EventId { get; set; }

        public int? OrderStatusId { get; set; }

        public virtual Event Event { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual Venue Venue { get; set; }

        // Fusha për multi-tenancy
        public string TenantId { get; set; }
    }
}
