using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace sshBackend1.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<StartupPlaceholder>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            // Use the real application setup but without depending on Program.cs
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<StartupPlaceholder>(); // Your actual Startup class
                });
        }
    }

    // Marker class just to provide a type reference
    public class StartupPlaceholder { }
}
