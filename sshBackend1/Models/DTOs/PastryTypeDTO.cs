using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
    public class PastryTypeDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PastryTypeId { get; set; }

        public string TypeName { get; set; }

        public virtual ICollection<Pastry> Pastries { get; set; } = new List<Pastry>();

        // Fusha për multi-tenancy
        public string TenantId { get; set; }
    }
}
