using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sshBackend1.Migrations
{
    /// <inheritdoc />
    public partial class UpToDateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "VenueTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "VenueOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "TableStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Tables",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "RestaurantStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "PlaylistItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "PerformerTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "PastryType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "PastryShops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "PastryOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Pastries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "PartnerStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "OrderStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "MusicProviders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "MusicProviderOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "MenuTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "MenuOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "GuestStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "FlowerArrangementTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "FlowerArrangements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "FlowerArrangementOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Florists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "VenueTypes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "VenueOrders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "TableStatuses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "RestaurantStatuses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PlaylistItems");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PerformerTypes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PastryType");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PastryShops");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PastryOrders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Pastries");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PartnerStatuses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "MusicProviders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "MusicProviderOrders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "MenuTypes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "MenuOrders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GuestStatuses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "FlowerArrangementTypes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "FlowerArrangements");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "FlowerArrangementOrders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Florists");

            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Events");
        }
    }
}
