using Microsoft.AspNetCore.Identity;
using NLog.Web.LayoutRenderers;
using System;
using sshBackend1.Models;

namespace sshBackend1.Models
{
    public class Users : IdentityUser
    {
        private int UserId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfilePictureUrl { get; set; }

        // Fusha për multi-tenancy
        public string TenantId { get; set; }

    }
}
