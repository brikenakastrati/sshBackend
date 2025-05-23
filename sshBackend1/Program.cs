
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using sshBackend1;

using sshBackend1.Data;
using sshBackend1.Helpers;
using sshBackend1.Middleware;
using sshBackend1.Models;
using sshBackend1.Repository;
using sshBackend1.Repository.IRepository;
using sshBackend1.Services;
using sshBackend1.Services.IServices;

using System.Text;



var builder = WebApplication.CreateBuilder(args);

// ----------------- Database -----------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")));

// ----------------- Identity -----------------
//builder.Services.AddIdentity<Users, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

// ----------------- Repositories -----------------
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IFloristRepository, FloristRepository>();
builder.Services.AddScoped<IVenueProviderRepository, VenueProviderRepository>();
builder.Services.AddScoped<IVenueTypeRepository, VenueTypeRepository>();
builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<IVenueOrderRepository, VenueOrderRepository>();
builder.Services.AddScoped<IFlowerArrangementTypeRepository, FlowerArrangementTypeRepository>();
builder.Services.AddScoped<IFlowerArrangementOrderRepository, FlowerArrangementOrderRepository>();
builder.Services.AddScoped<IFlowerArrangementRepository, FlowerArrangementRepository>();
//builder.Services.AddScoped<IRestaurantStatusRepository, RestaurantStatusRepository>();
//builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
//builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IPerformerTypeRepository, PerformerTypeRepository>();
builder.Services.AddScoped<IMusicProviderRepository, MusicProviderRepository>();
builder.Services.AddScoped<IMusicProviderOrderRepository, MusicProviderOrderRepository>();
builder.Services.AddScoped<IMenuTypeRepository, MenuTypeRepository>();
builder.Services.AddScoped<IMenuOrderRepository, MenuOrderRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IPlaylistItemRepository, PlaylistItemRepository>();
builder.Services.AddScoped<IPartnerStatusRepository, PartnerStatusRepository>();
builder.Services.AddScoped<IPastryShopRepository, PastryShopRepository>();
builder.Services.AddScoped<IPastryRepository, PastryRepository>();
builder.Services.AddScoped<IPastryOrderRepository, PastryOrderRepository>();
builder.Services.AddScoped<IPastryTypeRepository, PastryTypeRepository>();
//builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<JwtTokenHelper>();



builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// ----------------- AutoMapper -----------------
builder.Services.AddAutoMapper(typeof(MappingConfig));

// ----------------- Jwt Token Helper -----------------


// ----------------- Memory Cache -----------------
builder.Services.AddMemoryCache();

// ----------------- JWT Authentication -----------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("Key");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });



// ----------------- Authorization -----------------
builder.Services.AddAuthorization();

// ----------------- Controllers -----------------
builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

// ----------------- Swagger -----------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Event Planner",
        Description = "API to manage Event Planner",
        TermsOfService = new Uri("https://localhost:7197/terms_and_conditions"),
        Contact = new OpenApiContact
        {
            Name = "Dotnetmastery",
            Url = new Uri("https://dotnetmastery.com")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://localhost:7197/license")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


// ----------------- Build the App -----------------
var app = builder.Build();


// ----------------- Middleware Pipeline -----------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Event_Planner");
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<TenantMiddleware>();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
async Task SeedRolesAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "ADMIN", "CLIENT", "VENDOR" };

    foreach (var role in roles)
    {
        var roleExists = await roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}




app.MapControllers();
app.Run();