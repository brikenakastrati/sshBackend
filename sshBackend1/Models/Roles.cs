using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
