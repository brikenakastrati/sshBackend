using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sshBackend1.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_Tables_TableId",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_Menu_Restaurants_RestaurantId",
                table: "Menu");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_RestaurantStatuses_RestaurantStatusId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Venues_VenueId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Events_EventId",
                table: "Tables");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Venues_VenueId",
                table: "Tables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tables",
                table: "Tables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RestaurantStatuses",
                table: "RestaurantStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants");

            migrationBuilder.RenameTable(
                name: "Tables",
                newName: "Table");

            migrationBuilder.RenameTable(
                name: "RestaurantStatuses",
                newName: "RestaurantStatus");

            migrationBuilder.RenameTable(
                name: "Restaurants",
                newName: "Restaurant");

            migrationBuilder.RenameIndex(
                name: "IX_Tables_VenueId",
                table: "Table",
                newName: "IX_Table_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Tables_EventId",
                table: "Table",
                newName: "IX_Table_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_VenueId",
                table: "Restaurant",
                newName: "IX_Restaurant_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_RestaurantStatusId",
                table: "Restaurant",
                newName: "IX_Restaurant_RestaurantStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Table",
                table: "Table",
                column: "TableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestaurantStatus",
                table: "RestaurantStatus",
                column: "RestaurantStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurant",
                table: "Restaurant",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_Table_TableId",
                table: "Guests",
                column: "TableId",
                principalTable: "Table",
                principalColumn: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_Restaurant_RestaurantId",
                table: "Menu",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_RestaurantStatus_RestaurantStatusId",
                table: "Restaurant",
                column: "RestaurantStatusId",
                principalTable: "RestaurantStatus",
                principalColumn: "RestaurantStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Venues_VenueId",
                table: "Restaurant",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "VenueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Table_Events_EventId",
                table: "Table",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Table_Venues_VenueId",
                table: "Table",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "VenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_Table_TableId",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_Menu_Restaurant_RestaurantId",
                table: "Menu");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_RestaurantStatus_RestaurantStatusId",
                table: "Restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Venues_VenueId",
                table: "Restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Table_Events_EventId",
                table: "Table");

            migrationBuilder.DropForeignKey(
                name: "FK_Table_Venues_VenueId",
                table: "Table");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Table",
                table: "Table");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RestaurantStatus",
                table: "RestaurantStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurant",
                table: "Restaurant");

            migrationBuilder.RenameTable(
                name: "Table",
                newName: "Tables");

            migrationBuilder.RenameTable(
                name: "RestaurantStatus",
                newName: "RestaurantStatuses");

            migrationBuilder.RenameTable(
                name: "Restaurant",
                newName: "Restaurants");

            migrationBuilder.RenameIndex(
                name: "IX_Table_VenueId",
                table: "Tables",
                newName: "IX_Tables_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Table_EventId",
                table: "Tables",
                newName: "IX_Tables_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurant_VenueId",
                table: "Restaurants",
                newName: "IX_Restaurants_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurant_RestaurantStatusId",
                table: "Restaurants",
                newName: "IX_Restaurants_RestaurantStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tables",
                table: "Tables",
                column: "TableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestaurantStatuses",
                table: "RestaurantStatuses",
                column: "RestaurantStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_Tables_TableId",
                table: "Guests",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_Restaurants_RestaurantId",
                table: "Menu",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_RestaurantStatuses_RestaurantStatusId",
                table: "Restaurants",
                column: "RestaurantStatusId",
                principalTable: "RestaurantStatuses",
                principalColumn: "RestaurantStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Venues_VenueId",
                table: "Restaurants",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "VenueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Events_EventId",
                table: "Tables",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Venues_VenueId",
                table: "Tables",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "VenueId");
        }
    }
}
