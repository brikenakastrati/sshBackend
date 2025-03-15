using EFCoreMigrationsDemo.Data;
using Microsoft.EntityFrameworkCore;
using sshBackend1.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext to the container using the connection string from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity to use the custom Users model
builder.Services.AddDefaultIdentity<Users>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add other services, such as controllers, etc.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
