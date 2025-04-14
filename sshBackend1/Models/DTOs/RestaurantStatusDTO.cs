namespace sshBackend1.Models.DTOs
{
    public class RestaurantStatusDTO
    {
        public int RestaurantStatusId { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public DateTime LastUpdated { get; set; } 

        public string TenantId { get; set; }
    }
}
