using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sshBackend1.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTableGuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_Restaurant_RestaurantId",
                table: "Menu");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "RestaurantStatus");

            migrationBuilder.DropIndex(
                name: "IX_Menu_RestaurantId",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Menu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Menu",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RestaurantStatus",
                columns: table => new
                {
                    RestaurantStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantStatus", x => x.RestaurantStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false),
                    TableStatusId = table.Column<int>(type: "int", nullable: true),
                    VenueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.TableId);
                    table.ForeignKey(
                        name: "FK_Table_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK_Table_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId");
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantStatusId = table.Column<int>(type: "int", nullable: true),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.RestaurantId);
                    table.ForeignKey(
                        name: "FK_Restaurant_RestaurantStatus_RestaurantStatusId",
                        column: x => x.RestaurantStatusId,
                        principalTable: "RestaurantStatus",
                        principalColumn: "RestaurantStatusId");
                    table.ForeignKey(
                        name: "FK_Restaurant_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    GuestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    TableId = table.Column<int>(type: "int", nullable: true),
                    GuestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuestSecondName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuestStatus = table.Column<int>(type: "int", nullable: true),
                    GuestSurname = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                        name: "FK_Guests_Table_TableId",
                        column: x => x.TableId,
                        principalTable: "Table",
                        principalColumn: "TableId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menu_RestaurantId",
                table: "Menu",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_EventId",
                table: "Guests",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_TableId",
                table: "Guests",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_RestaurantStatusId",
                table: "Restaurant",
                column: "RestaurantStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_VenueId",
                table: "Restaurant",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Table_EventId",
                table: "Table",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Table_VenueId",
                table: "Table",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_Restaurant_RestaurantId",
                table: "Menu",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "RestaurantId");
        }
    }
}
