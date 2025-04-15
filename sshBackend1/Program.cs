using sshBackend1;
using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Repository.IRepository;
using sshBackend1.Repository;
using sshBackend1.Services;
using sshBackend1.Models;
using sshBackend1.Context;
using sshBackend1.Middleware;


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

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IContextProvider, HttpContextProvider>();


builder.Services.AddAutoMapper(typeof(MappingConfig));

// ? Add controllers and API formatters
builder.Services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build(); // ?? Do not register services after this!

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//  Vendos Middleware-in e Tenant-it para autorizimit
app.UseMiddleware<TenantMiddleware>();


app.UseAuthorization();
app.MapControllers();

app.Run();


