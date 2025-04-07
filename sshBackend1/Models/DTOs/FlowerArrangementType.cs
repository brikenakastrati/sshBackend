using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
    public class FlowerArrangementType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlowerArrangementTypeId { get; set; }

        public string Name { get; set; }

      
        public string TenantId { get; set; }
    }
}
