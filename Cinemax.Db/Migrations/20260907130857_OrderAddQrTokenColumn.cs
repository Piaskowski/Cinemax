using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemax.Db.Migrations
{
    /// <inheritdoc />
    public partial class OrderAddQrTokenColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrToken",
                table: "Orders",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_QrToken",
                table: "Orders",
                column: "QrToken",
                unique: true,
                filter: "[QrToken] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_QrToken",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "QrToken",
                table: "Orders");
        }
    }
}
