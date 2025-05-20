using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models.DTOs
{
    public class RoleDTO
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
