using Microsoft.AspNetCore.Identity;
using NLog.Web.LayoutRenderers;
using System;

namespace sshBackend1.Data.Models
{
    // This class inherits from IdentityUser to customize user properties
    public class Users : IdentityUser
    {
        // Custom properties
        private int UserId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfilePictureUrl { get; set; }

        // Add more custom properties as needed
    }
}
