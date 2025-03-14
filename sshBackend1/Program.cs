using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;

public class Program
{
    public static void Main(string[] args)
    {
        // Create and run the web host for the application
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices((context, services) =>
                {
                    // Add DbContext to the DI container
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(
                            context.Configuration.GetConnectionString("DefaultConnection")));  // Change if you're using another DB

                    // Add services for controllers (API)
                    services.AddControllers();

                    // Add CORS policy to allow frontend access
                    services.AddCors(options =>
                    {
                        options.AddPolicy("AllowFrontend", builder =>
                            builder.WithOrigins("http://localhost:3000")  
                                   .AllowAnyHeader()
                                   .AllowAnyMethod());
                    });

                    // Optionally add other services like authentication, logging, etc.
                });

                webBuilder.Configure(app =>
                {
                    var env = app.ApplicationServices.GetService<IWebHostEnvironment>();

                    // Configure middleware
                    if (env.IsDevelopment())
                    {
                        // Show detailed error pages in development
                        app.UseDeveloperExceptionPage();
                    }
                    else
                    {
                        // In production, use a more secure error handling strategy
                        app.UseExceptionHandler("/Home/Error");
                        app.UseHsts();
                    }

                    // Enable CORS middleware
                    app.UseCors("AllowFrontend");

                    // Enable routing
                    app.UseRouting();

                    // Enable authentication and authorization middleware (if needed)
                    app.UseAuthentication();
                    app.UseAuthorization();

                    // Map controller routes
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers(); // API controllers
                    });
                });
            });
}

