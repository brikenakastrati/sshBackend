using Microsoft.AspNetCore.Identity;

namespace sshBackend1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
