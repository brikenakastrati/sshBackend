using System;
using sshBackend1.Models;


namespace sshBackend1.Models
{
    public partial class RestaurantStatus
    {
        public int RestaurantStatusId { get; set; } // Primary key for the status
        public string Name { get; set; } // Name of the status (e.g., Open, Closed, Under Renovation)
        public string Description { get; set; } // A brief description of the status
        public DateTime LastUpdated { get; set; } // The last time the status was updated
                                                  
        // Fusha për multi-tenancy
        public string TenantId { get; set; }
    }
}
