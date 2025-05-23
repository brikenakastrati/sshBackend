using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sshBackend1.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedOrderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowerArrangementOrders_Events_EventId",
                table: "FlowerArrangementOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowerArrangementOrders_OrderStatuses_OrderStatusId",
                table: "FlowerArrangementOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicProviderOrders_Events_EventId",
                table: "MusicProviderOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicProviderOrders_OrderStatuses_OrderStatusId",
                table: "MusicProviderOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PastryOrders_Events_EventId",
                table: "PastryOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PastryOrders_OrderStatuses_OrderStatusId",
                table: "PastryOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_VenueOrders_Events_EventId",
                table: "VenueOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_VenueOrders_OrderStatuses_OrderStatusId",
                table: "VenueOrders");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "VenueOrders");

            migrationBuilder.DropColumn(
                name: "AgencyFee",
                table: "VenueOrders");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "VenueOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "VenueOrders");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "PastryOrders");

            migrationBuilder.DropColumn(
                name: "OrderDescription",
                table: "PastryOrders");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "MusicProviderOrders");

            migrationBuilder.DropColumn(
                name: "MusicProviderAddress",
                table: "MusicProviderOrders");

            migrationBuilder.DropColumn(
                name: "OrderName",
                table: "MusicProviderOrders");

            migrationBuilder.RenameColumn(
                name: "OrderName",
                table: "PastryOrders",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "MusicProviderOrders",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "OrderName",
                table: "FlowerArrangementOrders",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatusId",
                table: "VenueOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "VenueOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "VenueOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "VenueProviderId",
                table: "VenueOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatusId",
                table: "PastryOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OrderPrice",
                table: "PastryOrders",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "PastryOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "AgencyFee",
                table: "PastryOrders",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "OrderShopShopId",
                table: "PastryOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "PastryOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatusId",
                table: "MusicProviderOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OrderPrice",
                table: "MusicProviderOrders",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "MusicProviderOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "MusicProviderOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "AgencyFee",
                table: "MusicProviderOrders",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "MusicProviderId",
                table: "MusicProviderOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatusId",
                table: "FlowerArrangementOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "FlowerArrangementOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FloristId",
                table: "FlowerArrangementOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VenueOrders_VenueProviderId",
                table: "VenueOrders",
                column: "VenueProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_PastryOrders_OrderShopShopId",
                table: "PastryOrders",
                column: "OrderShopShopId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicProviderOrders_MusicProviderId",
                table: "MusicProviderOrders",
                column: "MusicProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowerArrangementOrders_FloristId",
                table: "FlowerArrangementOrders",
                column: "FloristId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowerArrangementOrders_Events_EventId",
                table: "FlowerArrangementOrders",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowerArrangementOrders_Florists_FloristId",
                table: "FlowerArrangementOrders",
                column: "FloristId",
                principalTable: "Florists",
                principalColumn: "FloristId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowerArrangementOrders_OrderStatuses_OrderStatusId",
                table: "FlowerArrangementOrders",
                column: "OrderStatusId",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicProviderOrders_Events_EventId",
                table: "MusicProviderOrders",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicProviderOrders_MusicProviders_MusicProviderId",
                table: "MusicProviderOrders",
                column: "MusicProviderId",
                principalTable: "MusicProviders",
                principalColumn: "MusicProviderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicProviderOrders_OrderStatuses_OrderStatusId",
                table: "MusicProviderOrders",
                column: "OrderStatusId",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PastryOrders_Events_EventId",
                table: "PastryOrders",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PastryOrders_OrderStatuses_OrderStatusId",
                table: "PastryOrders",
                column: "OrderStatusId",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PastryOrders_PastryShops_OrderShopShopId",
                table: "PastryOrders",
                column: "OrderShopShopId",
                principalTable: "PastryShops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VenueOrders_Events_EventId",
                table: "VenueOrders",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VenueOrders_OrderStatuses_OrderStatusId",
                table: "VenueOrders",
                column: "OrderStatusId",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VenueOrders_VenueProviders_VenueProviderId",
                table: "VenueOrders",
                column: "VenueProviderId",
                principalTable: "VenueProviders",
                principalColumn: "VenueProviderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowerArrangementOrders_Events_EventId",
                table: "FlowerArrangementOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowerArrangementOrders_Florists_FloristId",
                table: "FlowerArrangementOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowerArrangementOrders_OrderStatuses_OrderStatusId",
                table: "FlowerArrangementOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicProviderOrders_Events_EventId",
                table: "MusicProviderOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicProviderOrders_MusicProviders_MusicProviderId",
                table: "MusicProviderOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicProviderOrders_OrderStatuses_OrderStatusId",
                table: "MusicProviderOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PastryOrders_Events_EventId",
                table: "PastryOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PastryOrders_OrderStatuses_OrderStatusId",
                table: "PastryOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PastryOrders_PastryShops_OrderShopShopId",
                table: "PastryOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_VenueOrders_Events_EventId",
                table: "VenueOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_VenueOrders_OrderStatuses_OrderStatusId",
                table: "VenueOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_VenueOrders_VenueProviders_VenueProviderId",
                table: "VenueOrders");

            migrationBuilder.DropIndex(
                name: "IX_VenueOrders_VenueProviderId",
                table: "VenueOrders");

            migrationBuilder.DropIndex(
                name: "IX_PastryOrders_OrderShopShopId",
                table: "PastryOrders");

            migrationBuilder.DropIndex(
                name: "IX_MusicProviderOrders_MusicProviderId",
                table: "MusicProviderOrders");

            migrationBuilder.DropIndex(
                name: "IX_FlowerArrangementOrders_FloristId",
                table: "FlowerArrangementOrders");

            migrationBuilder.DropColumn(
                name: "VenueProviderId",
                table: "VenueOrders");

            migrationBuilder.DropColumn(
                name: "OrderShopShopId",
                table: "PastryOrders");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "PastryOrders");

            migrationBuilder.DropColumn(
                name: "MusicProviderId",
                table: "MusicProviderOrders");

            migrationBuilder.DropColumn(
                name: "FloristId",
                table: "FlowerArrangementOrders");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PastryOrders",
                newName: "OrderName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MusicProviderOrders",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FlowerArrangementOrders",
                newName: "OrderName");

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatusId",
                table: "VenueOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "VenueOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "VenueOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "VenueOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "AgencyFee",
                table: "VenueOrders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "VenueOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "VenueOrders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatusId",
                table: "PastryOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "OrderPrice",
                table: "PastryOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "PastryOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "AgencyFee",
                table: "PastryOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "PastryOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderDescription",
                table: "PastryOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatusId",
                table: "MusicProviderOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "OrderPrice",
                table: "MusicProviderOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "MusicProviderOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "MusicProviderOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "AgencyFee",
                table: "MusicProviderOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "MusicProviderOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MusicProviderAddress",
                table: "MusicProviderOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderName",
                table: "MusicProviderOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatusId",
                table: "FlowerArrangementOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "FlowerArrangementOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowerArrangementOrders_Events_EventId",
                table: "FlowerArrangementOrders",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowerArrangementOrders_OrderStatuses_OrderStatusId",
                table: "FlowerArrangementOrders",
                column: "OrderStatusId",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicProviderOrders_Events_EventId",
                table: "MusicProviderOrders",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicProviderOrders_OrderStatuses_OrderStatusId",
                table: "MusicProviderOrders",
                column: "OrderStatusId",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PastryOrders_Events_EventId",
                table: "PastryOrders",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_PastryOrders_OrderStatuses_OrderStatusId",
                table: "PastryOrders",
                column: "OrderStatusId",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_VenueOrders_Events_EventId",
                table: "VenueOrders",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_VenueOrders_OrderStatuses_OrderStatusId",
                table: "VenueOrders",
                column: "OrderStatusId",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusId");
        }
    }
}
