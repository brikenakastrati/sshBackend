using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sshBackend1.Data.Models;

namespace EFCoreMigrationsDemo.Data
{
    // Use the Users class for IdentityUser customization
    public class ApplicationDbContext : IdentityDbContext<Users>  // Use 'Users' as the custom user class
    {
        private readonly IConfiguration _configuration;

        // Constructor with DbContextOptions and IConfiguration injection
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        // Parameterless constructor for EF Core at design time (for migrations)
        public ApplicationDbContext() { }

        // Configure the DbContext to use SQL Server with the connection string from appsettings.json
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Get the connection string from appsettings.json
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        // Optionally: Add DbSet properties for other models (like Product, Orders, etc.)
        // public DbSet<Product> Products { get; set; }
    }
}
