using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using sshBackend1.Models;
namespace sshBackend1.Data

{
    public class ApplicationDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Event> Events { get; set; }

        public DbSet<Florist> Florists { get; set; }
        public DbSet<FlowerArrangement> FlowerArrangements { get; set; }
        public DbSet<FlowerArrangementOrder> FlowerArrangementOrders { get; set; }
        public DbSet<FlowerArrangementType> FlowerArrangementTypes { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<GuestStatus> GuestStatuses { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuOrder> MenuOrders { get; set; }
        public DbSet<MenuType> MenuTypes { get; set; }
        public DbSet<MusicProvider> MusicProviders { get; set; }
        public DbSet<MusicProviderOrder> MusicProviderOrders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PartnerStatus> PartnerStatuses { get; set; }
        public DbSet<Pastry> Pastries { get; set; }
        public DbSet<PastryShop> PastryShops { get; set; }
        public DbSet<PastryOrder> PastryOrders { get; set; }
        public DbSet<PastryType> PastryTypes { get; set; }
        public DbSet<PerformerType> PerformerTypes { get; set; }
        public DbSet<PlaylistItem> PlaylistItems { get; set; }
        public DbSet<Restaurant> Restaurants{ get; set; }
        public DbSet<RestaurantStatus> RestaurantStatuses{ get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<VenueOrder> VenueOrders { get; set; }
        public DbSet<VenueType> VenueTypes { get; set; }
        public DbSet<VenueProvider> VenueProviders { get; set; }

    }
}
