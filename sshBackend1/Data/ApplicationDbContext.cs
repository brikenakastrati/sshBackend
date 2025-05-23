﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using sshBackend1.Models;

namespace sshBackend1.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Florist> Florists { get; set; }
        public DbSet<FlowerArrangement> FlowerArrangements { get; set; }
        public DbSet<FlowerArrangementOrder> FlowerArrangementOrders { get; set; }
        public DbSet<FlowerArrangementType> FlowerArrangementTypes { get; set; }

    

       

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

       

        public DbSet<Venue> Venues { get; set; }
        public DbSet<VenueOrder> VenueOrders { get; set; }
        public DbSet<VenueType> VenueTypes { get; set; }
        public DbSet<VenueProvider> VenueProviders { get; set; }

    }
}
