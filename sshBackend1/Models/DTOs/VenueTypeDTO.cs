namespace sshBackend1.Models.DTOs
{
    public class VenueTypeDTO
    {
        public int VenueTypeId { get; set; }

        public string Name { get; set; }


        // Fusha për multi-tenancy
        public string TenantId { get; set; }
    }
}
