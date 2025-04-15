using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sshBackend1.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventTypeId = table.Column<int>(type: "int", nullable: true),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "FlowerArrangementTypes",
                columns: table => new
                {
                    FlowerArrangementTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowerArrangementTypes", x => x.FlowerArrangementTypeId);
                });

            migrationBuilder.CreateTable(
                name: "GuestStatuses",
                columns: table => new
                {
                    GuestStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestStatusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestStatuses", x => x.GuestStatusId);
                });

            migrationBuilder.CreateTable(
                name: "MenuTypes",
                columns: table => new
                {
                    MenuTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuTypes", x => x.MenuTypeId);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    OrderStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderStatusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.OrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "PartnerStatuses",
                columns: table => new
                {
                    PartnerStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerStatuses", x => x.PartnerStatusId);
                });

            migrationBuilder.CreateTable(
                name: "PastryType",
                columns: table => new
                {
                    PastryTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastryType", x => x.PastryTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PerformerTypes",
                columns: table => new
                {
                    PerformerTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformerTypes", x => x.PerformerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantStatuses",
                columns: table => new
                {
                    RestaurantStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantStatuses", x => x.RestaurantStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VenueTypes",
                columns: table => new
                {
                    VenueTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueTypes", x => x.VenueTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FlowerArrangementOrders",
                columns: table => new
                {
                    FlowerArrangementOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgencyFee = table.Column<double>(type: "float", nullable: false),
                    OrderDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    OrderStatusId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowerArrangementOrders", x => x.FlowerArrangementOrderId);
                    table.ForeignKey(
                        name: "FK_FlowerArrangementOrders_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK_FlowerArrangementOrders_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "OrderStatusId");
                });

            migrationBuilder.CreateTable(
                name: "MenuOrders",
                columns: table => new
                {
                    MenuOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgencyFee = table.Column<double>(type: "float", nullable: false),
                    Allergents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IngreedientsForbiddenByReligion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalRequests = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    OrderStatusId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuOrders", x => x.MenuOrderId);
                    table.ForeignKey(
                        name: "FK_MenuOrders_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK_MenuOrders_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "OrderStatusId");
                });

            migrationBuilder.CreateTable(
                name: "MusicProviderOrders",
                columns: table => new
                {
                    MusicProviderOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgencyFee = table.Column<double>(type: "float", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MusicProviderAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    OrderStatusId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicProviderOrders", x => x.MusicProviderOrderId);
                    table.ForeignKey(
                        name: "FK_MusicProviderOrders_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK_MusicProviderOrders_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "OrderStatusId");
                });

            migrationBuilder.CreateTable(
                name: "PastryOrders",
                columns: table => new
                {
                    PastryOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgencyFee = table.Column<double>(type: "float", nullable: false),
                    OrderDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    OrderStatusId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastryOrders", x => x.PastryOrderId);
                    table.ForeignKey(
                        name: "FK_PastryOrders_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK_PastryOrders_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "OrderStatusId");
                });

            migrationBuilder.CreateTable(
                name: "Florists",
                columns: table => new
                {
                    FloristId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgencyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PartnerStatusId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Florists", x => x.FloristId);
                    table.ForeignKey(
                        name: "FK_Florists_PartnerStatuses_PartnerStatusId",
                        column: x => x.PartnerStatusId,
                        principalTable: "PartnerStatuses",
                        principalColumn: "PartnerStatusId");
                });

            migrationBuilder.CreateTable(
                name: "PastryShops",
                columns: table => new
                {
                    ShopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartnerStatusId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastryShops", x => x.ShopId);
                    table.ForeignKey(
                        name: "FK_PastryShops_PartnerStatuses_PartnerStatusId",
                        column: x => x.PartnerStatusId,
                        principalTable: "PartnerStatuses",
                        principalColumn: "PartnerStatusId");
                });

            migrationBuilder.CreateTable(
                name: "VenueProviders",
                columns: table => new
                {
                    VenueProviderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgencyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PartnerStatusId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueProviders", x => x.VenueProviderId);
                    table.ForeignKey(
                        name: "FK_VenueProviders_PartnerStatuses_PartnerStatusId",
                        column: x => x.PartnerStatusId,
                        principalTable: "PartnerStatuses",
                        principalColumn: "PartnerStatusId");
                });

            migrationBuilder.CreateTable(
                name: "MusicProviders",
                columns: table => new
                {
                    MusicProviderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgencyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PerformerTypeId = table.Column<int>(type: "int", nullable: true),
                    PartnerStatusId = table.Column<int>(type: "int", nullable: true),
                    BaseHourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicProviders", x => x.MusicProviderId);
                    table.ForeignKey(
                        name: "FK_MusicProviders_PartnerStatuses_PartnerStatusId",
                        column: x => x.PartnerStatusId,
                        principalTable: "PartnerStatuses",
                        principalColumn: "PartnerStatusId");
                    table.ForeignKey(
                        name: "FK_MusicProviders_PerformerTypes_PerformerTypeId",
                        column: x => x.PerformerTypeId,
                        principalTable: "PerformerTypes",
                        principalColumn: "PerformerTypeId");
                });

            migrationBuilder.CreateTable(
                name: "FlowerArrangements",
                columns: table => new
                {
                    FlowerArrangementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FloristId = table.Column<int>(type: "int", nullable: false),
                    FlowerArrangementTypeId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowerArrangements", x => x.FlowerArrangementId);
                    table.ForeignKey(
                        name: "FK_FlowerArrangements_Florists_FloristId",
                        column: x => x.FloristId,
                        principalTable: "Florists",
                        principalColumn: "FloristId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowerArrangements_FlowerArrangementTypes_FlowerArrangementTypeId",
                        column: x => x.FlowerArrangementTypeId,
                        principalTable: "FlowerArrangementTypes",
                        principalColumn: "FlowerArrangementTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pastries",
                columns: table => new
                {
                    PastryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PastryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: true),
                    PastryTypeId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pastries", x => x.PastryId);
                    table.ForeignKey(
                        name: "FK_Pastries_PastryShops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "PastryShops",
                        principalColumn: "ShopId");
                    table.ForeignKey(
                        name: "FK_Pastries_PastryType_PastryTypeId",
                        column: x => x.PastryTypeId,
                        principalTable: "PastryType",
                        principalColumn: "PastryTypeId");
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    VenueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VenueProviderId = table.Column<int>(type: "int", nullable: true),
                    VenueTypeId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.VenueId);
                    table.ForeignKey(
                        name: "FK_Venues_VenueProviders_VenueProviderId",
                        column: x => x.VenueProviderId,
                        principalTable: "VenueProviders",
                        principalColumn: "VenueProviderId");
                    table.ForeignKey(
                        name: "FK_Venues_VenueTypes_VenueTypeId",
                        column: x => x.VenueTypeId,
                        principalTable: "VenueTypes",
                        principalColumn: "VenueTypeId");
                });

            migrationBuilder.CreateTable(
                name: "PlaylistItems",
                columns: table => new
                {
                    PlaylistItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    MusicProviderId = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistItems", x => x.PlaylistItemId);
                    table.ForeignKey(
                        name: "FK_PlaylistItems_MusicProviders_MusicProviderId",
                        column: x => x.MusicProviderId,
                        principalTable: "MusicProviders",
                        principalColumn: "MusicProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantStatusId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.RestaurantId);
                    table.ForeignKey(
                        name: "FK_Restaurants_RestaurantStatuses_RestaurantStatusId",
                        column: x => x.RestaurantStatusId,
                        principalTable: "RestaurantStatuses",
                        principalColumn: "RestaurantStatusId");
                    table.ForeignKey(
                        name: "FK_Restaurants_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false),
                    TableStatusId = table.Column<int>(type: "int", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.TableId);
                    table.ForeignKey(
                        name: "FK_Tables_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK_Tables_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId");
                });

            migrationBuilder.CreateTable(
                name: "VenueOrders",
                columns: table => new
                {
                    VenueOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgencyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    OrderStatusId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueOrders", x => x.VenueOrderId);
                    table.ForeignKey(
                        name: "FK_VenueOrders_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK_VenueOrders_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "OrderStatusId");
                    table.ForeignKey(
                        name: "FK_VenueOrders_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    MenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CateringId = table.Column<int>(type: "int", nullable: true),
                    MenuTypeId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.MenuId);
                    table.ForeignKey(
                        name: "FK_Menu_MenuTypes_MenuTypeId",
                        column: x => x.MenuTypeId,
                        principalTable: "MenuTypes",
                        principalColumn: "MenuTypeId");
                    table.ForeignKey(
                        name: "FK_Menu_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId");
                });

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    GuestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuestSecondName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuestSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuestStatusId = table.Column<int>(type: "int", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    TableId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.GuestId);
                    table.ForeignKey(
                        name: "FK_Guests_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK_Guests_GuestStatuses_GuestStatusId",
                        column: x => x.GuestStatusId,
                        principalTable: "GuestStatuses",
                        principalColumn: "GuestStatusId");
                    table.ForeignKey(
                        name: "FK_Guests_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "TableId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Florists_PartnerStatusId",
                table: "Florists",
                column: "PartnerStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowerArrangementOrders_EventId",
                table: "FlowerArrangementOrders",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowerArrangementOrders_OrderStatusId",
                table: "FlowerArrangementOrders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowerArrangements_FloristId",
                table: "FlowerArrangements",
                column: "FloristId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowerArrangements_FlowerArrangementTypeId",
                table: "FlowerArrangements",
                column: "FlowerArrangementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_EventId",
                table: "Guests",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_GuestStatusId",
                table: "Guests",
                column: "GuestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_TableId",
                table: "Guests",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_MenuTypeId",
                table: "Menu",
                column: "MenuTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_RestaurantId",
                table: "Menu",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuOrders_EventId",
                table: "MenuOrders",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuOrders_OrderStatusId",
                table: "MenuOrders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicProviderOrders_EventId",
                table: "MusicProviderOrders",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicProviderOrders_OrderStatusId",
                table: "MusicProviderOrders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicProviders_PartnerStatusId",
                table: "MusicProviders",
                column: "PartnerStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicProviders_PerformerTypeId",
                table: "MusicProviders",
                column: "PerformerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pastries_PastryTypeId",
                table: "Pastries",
                column: "PastryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pastries_ShopId",
                table: "Pastries",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_PastryOrders_EventId",
                table: "PastryOrders",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_PastryOrders_OrderStatusId",
                table: "PastryOrders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PastryShops_PartnerStatusId",
                table: "PastryShops",
                column: "PartnerStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistItems_MusicProviderId",
                table: "PlaylistItems",
                column: "MusicProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_RestaurantStatusId",
                table: "Restaurants",
                column: "RestaurantStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_VenueId",
                table: "Restaurants",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_EventId",
                table: "Tables",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_VenueId",
                table: "Tables",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueOrders_EventId",
                table: "VenueOrders",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueOrders_OrderStatusId",
                table: "VenueOrders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueOrders_VenueId",
                table: "VenueOrders",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueProviders_PartnerStatusId",
                table: "VenueProviders",
                column: "PartnerStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Venues_VenueProviderId",
                table: "Venues",
                column: "VenueProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Venues_VenueTypeId",
                table: "Venues",
                column: "VenueTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowerArrangementOrders");

            migrationBuilder.DropTable(
                name: "FlowerArrangements");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "MenuOrders");

            migrationBuilder.DropTable(
                name: "MusicProviderOrders");

            migrationBuilder.DropTable(
                name: "Pastries");

            migrationBuilder.DropTable(
                name: "PastryOrders");

            migrationBuilder.DropTable(
                name: "PlaylistItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VenueOrders");

            migrationBuilder.DropTable(
                name: "Florists");

            migrationBuilder.DropTable(
                name: "FlowerArrangementTypes");

            migrationBuilder.DropTable(
                name: "GuestStatuses");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "MenuTypes");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "PastryShops");

            migrationBuilder.DropTable(
                name: "PastryType");

            migrationBuilder.DropTable(
                name: "MusicProviders");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "RestaurantStatuses");

            migrationBuilder.DropTable(
                name: "Venues");

            migrationBuilder.DropTable(
                name: "PerformerTypes");

            migrationBuilder.DropTable(
                name: "VenueProviders");

            migrationBuilder.DropTable(
                name: "VenueTypes");

            migrationBuilder.DropTable(
                name: "PartnerStatuses");
        }
    }
}
