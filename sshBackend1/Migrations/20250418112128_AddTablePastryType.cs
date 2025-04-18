using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sshBackend1.Migrations
{
    /// <inheritdoc />
    public partial class AddTablePastryType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pastries_PastryType_PastryTypeId",
                table: "Pastries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PastryType",
                table: "PastryType");

            migrationBuilder.RenameTable(
                name: "PastryType",
                newName: "PastryTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PastryTypes",
                table: "PastryTypes",
                column: "PastryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pastries_PastryTypes_PastryTypeId",
                table: "Pastries",
                column: "PastryTypeId",
                principalTable: "PastryTypes",
                principalColumn: "PastryTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pastries_PastryTypes_PastryTypeId",
                table: "Pastries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PastryTypes",
                table: "PastryTypes");

            migrationBuilder.RenameTable(
                name: "PastryTypes",
                newName: "PastryType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PastryType",
                table: "PastryType",
                column: "PastryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pastries_PastryType_PastryTypeId",
                table: "Pastries",
                column: "PastryTypeId",
                principalTable: "PastryType",
                principalColumn: "PastryTypeId");
        }
    }
}
