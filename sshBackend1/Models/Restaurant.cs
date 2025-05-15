using System;
using System.Collections.Generic;

namespace sshBackend1.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; } // Unique identifier for the restaurant
        public string RestaurantName { get; set; } // Name of the restaurant
        public string Address { get; set; } // Restaurant's physical address
        public string PhoneNumber { get; set; } // Restaurant's contact number
        public int? RestaurantStatusId { get; set; } // Foreign key for the restaurant's status (open/closed)



       
        public int VenueId { get; set; } 
        public virtual Venue Venue { get; set; }
        public virtual RestaurantStatus RestaurantStatus { get; set; }

        public virtual ICollection<Menu> Menu { get; set; } = new List<Menu>();
    }
}
