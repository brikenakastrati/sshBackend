using Microsoft.AspNetCore.Identity;
using NLog.Web.LayoutRenderers;
using System;
using sshBackend1.Models;
using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models
{
    public class Users : IdentityUser
    {
        [Key]
        public string Id { get; set; }

        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfilePictureUrl { get; set; }

        public string TenantId { get; set; }

        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }

        public string Email { get; set; }
        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }

        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public string Password { get; set; } // ⚠️ Rekomandohet të mos 

    }
}
