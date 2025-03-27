using System;
using System.Collections.Generic;

namespace sshBackend1.Models
{
    public partial class Restaurant
    {
        public int RestaurantId { get; set; } // Unique identifier for the restaurant
        public string RestaurantName { get; set; } // Name of the restaurant
        public string Address { get; set; } // Restaurant's physical address
        public string PhoneNumber { get; set; } // Restaurant's contact number
        public int? RestaurantStatusId { get; set; } // Foreign key for the restaurant's status (open/closed)

        // Fusha për multi-tenancy
        public string TenantId { get; set; }

        // Foreign Key to connect the Restaurant to the Venue
        public int VenueId { get; set; } // Foreign Key to Venue
        public virtual Venue Venue { get; set; } // Navigation property to access the related Venue

        // Navigation property to the RestaurantStatus
        public virtual RestaurantStatus RestaurantStatus { get; set; }

        // Collection of MenuItems related to this restaurant
        public virtual ICollection<Menu> Menu { get; set; } = new List<Menu>();
    }
}
