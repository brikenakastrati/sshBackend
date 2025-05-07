using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using sshBackend1.Context;
using sshBackend1.Data;
using sshBackend1.Middleware;
using sshBackend1.Repository;
using sshBackend1.Repository.IRepository;
using sshBackend1.Services;
using sshBackend1.Services.IServices;

namespace sshBackend1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Configure services
        public void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultSQLConnection")));

            // Add repositories
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IFloristRepository, FloristRepository>();
            services.AddScoped<IVenueProviderRepository, VenueProviderRepository>();
            services.AddScoped<IVenueTypeRepository, VenueTypeRepository>();
            services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
            services.AddScoped<IVenueRepository, VenueRepository>();
            services.AddScoped<IVenueOrderRepository, VenueOrderRepository>();
            services.AddScoped<IFlowerArrangementTypeRepository, FlowerArrangementTypeRepository>();
            services.AddScoped<IFlowerArrangementOrderRepository, FlowerArrangementOrderRepository>();
            services.AddScoped<IFlowerArrangementRepository, FlowerArrangementRepository>();
            services.AddScoped<IRestaurantStatusRepository, RestaurantStatusRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<ITableRepository, TableRepository>();
            services.AddScoped<IPerformerTypeRepository, PerformerTypeRepository>();
            services.AddScoped<IMusicProviderRepository, MusicProviderRepository>();
            services.AddScoped<IMusicProviderOrderRepository, MusicProviderOrderRepository>();
            services.AddScoped<IMenuTypeRepository, MenuTypeRepository>();
            services.AddScoped<IMenuOrderRepository, MenuOrderRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IPlaylistItemRepository, PlaylistItemRepository>();
            services.AddScoped<IPartnerStatusRepository, PartnerStatusRepository>();
            services.AddScoped<IPastryShopRepository, PastryShopRepository>();
            services.AddScoped<IPastryRepository, PastryRepository>();
            services.AddScoped<IPastryOrderRepository, PastryOrderRepository>();
            services.AddScoped<IPastryTypeRepository, PastryTypeRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();
            services.AddScoped<ICacheService, MemoryCacheService>();

            // Add other services
            services.AddHttpContextAccessor();
            services.AddScoped<IContextProvider, HttpContextProvider>();
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddMemoryCache();

            // Add controllers and formatters
            services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

            // Add Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
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
            });
        }

        // Modify the Configure method
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Event_Planner");
                });
            }

            app.UseHttpsRedirection();

            // Add UseRouting before UseEndpoints
            app.UseRouting();

            // Add custom middleware
            app.UseMiddleware<TenantMiddleware>();

            app.UseAuthorization();

            // Map endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

