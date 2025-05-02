using sshBackend1;
using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Repository.IRepository;
using sshBackend1.Repository;

using sshBackend1.Models;
using sshBackend1.Context;
using sshBackend1.Middleware;
using System.Security.Cryptography.Xml;
using Microsoft.OpenApi.Models;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Services.IServices;
using sshBackend1.Services;



var builder = WebApplication.CreateBuilder(args);

// ? Add services to the container **before** calling `builder.Build()`
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")));

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
builder.Services.AddScoped<IRestaurantStatusRepository, RestaurantStatusRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
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
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IContextProvider, HttpContextProvider>();


builder.Services.AddAutoMapper(typeof(MappingConfig));
//builder.Services.AddApiVersioning(options =>
//{
//    options.AssumeDefaultVersionWhenUnspecified = true;
//    options.DefaultApiVersion = new ApiVersion(1, 0);
//    options.ReportApiVersions = true;
//});

//builder.Services.AddVersionedApiExplorer(options =>
//{
//    options.GroupNameFormat = "'v'VVV";
//    options.SubstituteApiVersionInUrl = true;
//});

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

// ? Add controllers and API formatters
builder.Services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    //{
    //    Description =
    //    "JWT Authorization header using the Bearer Scheme. \r\n\n\n" +
    //    "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n" +
    //    "Example: \"Bearer 12345abcdef\"",
    //    Name = "Authorization",
    //    In = ParameterLocation.Header,
    //    Scheme = "Bearer"

    //});
    //options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //                        {
    //                            Type = ReferenceType.SecurityScheme,
    //                            Id = "Bearer"
    //                        },
    //            Scheme = "oauth2",
    //            Name = "Bearer",
    //            In = ParameterLocation.Header
    //        },
    //        new List<string>()
    //    }
    //});
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
});
builder.Services.AddMemoryCache();


var app = builder.Build(); // ?? Do not register services after this!

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Event_Planner");
    });
}

app.UseHttpsRedirection();

//  Vendos Middleware-in e Tenant-it para autorizimit
app.UseMiddleware<TenantMiddleware>();


app.UseAuthorization();
app.MapControllers();

app.Run();


