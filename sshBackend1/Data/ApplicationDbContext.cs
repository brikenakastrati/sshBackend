using Microsoft.EntityFrameworkCore;

namespace sshBackend1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

       // public DbSet<Product> Products { get; set; }
    }

}
