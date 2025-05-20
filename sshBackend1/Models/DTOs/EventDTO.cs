using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
        public class EventDTO
        {
            public int EventId { get; set; }

            public string EventName { get; set; }

            public string EventType { get; set; }

            public DateTime? EventDate { get; set; }

            public string ApplicationUserId { get; set; } 
        }
    

}
