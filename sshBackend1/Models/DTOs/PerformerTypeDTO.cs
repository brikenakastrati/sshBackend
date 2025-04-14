using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models.DTOs
{
    public class PerformerTypeDTO
    {
        [Key]
        public int PerformerTypeId { get; set; }

        public string Name { get; set; }


        public string TenantId { get; set; }
    }
}
