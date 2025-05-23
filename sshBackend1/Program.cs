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

// 1) Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")));

// 2) Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 3) Repositories
builder.Services.AddScoped<IVenueOrderRepository, VenueOrderRepository>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<IVenueProviderRepository, VenueProviderRepository>();
builder.Services.AddScoped<IVenueTypeRepository, VenueTypeRepository>();
builder.Services.AddScoped<IFloristRepository, FloristRepository>();
builder.Services.AddScoped<IFlowerArrangementRepository, FlowerArrangementRepository>();
builder.Services.AddScoped<IFlowerArrangementTypeRepository, FlowerArrangementTypeRepository>();
builder.Services.AddScoped<IFlowerArrangementOrderRepository, FlowerArrangementOrderRepository>();
builder.Services.AddScoped<IMusicProviderRepository, MusicProviderRepository>();
builder.Services.AddScoped<IMusicProviderOrderRepository, MusicProviderOrderRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuTypeRepository, MenuTypeRepository>();
builder.Services.AddScoped<IMenuOrderRepository, MenuOrderRepository>();
builder.Services.AddScoped<IPlaylistItemRepository, PlaylistItemRepository>();
builder.Services.AddScoped<IPastryShopRepository, PastryShopRepository>();
builder.Services.AddScoped<IPastryRepository, PastryRepository>();
builder.Services.AddScoped<IPastryTypeRepository, PastryTypeRepository>();
builder.Services.AddScoped<IPastryOrderRepository, PastryOrderRepository>();
builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
builder.Services.AddScoped<IPartnerStatusRepository, PartnerStatusRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

// 4) Caching & AutoMapper
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<JwtTokenHelper>();

// 5) JWT Authentication
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

// 6) Authorization
builder.Services.AddAuthorization();

// 7) Controllers + JSON + XML
builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

// 8) Swagger (with JWT support)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "sshBackend1 API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Bearer scheme. Enter: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// 9) Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "sshBackend1 API v1"));
}

app.UseHttpsRedirection();

// If you have the tenant middleware:
app.UseMiddleware<TenantMiddleware>();

// **Make sure these are in this order:**
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
