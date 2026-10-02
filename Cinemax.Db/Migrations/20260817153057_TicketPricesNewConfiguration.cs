using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemax.Db.Migrations
{
    /// <inheritdoc />
    public partial class TicketPricesNewConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketPrices_ScreeningType_TicketType",
                table: "TicketPrices");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPrices_ScreeningType_TicketType_SeatType",
                table: "TicketPrices",
                columns: new[] { "ScreeningType", "TicketType", "SeatType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketPrices_ScreeningType_TicketType_SeatType",
                table: "TicketPrices");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPrices_ScreeningType_TicketType",
                table: "TicketPrices",
                columns: new[] { "ScreeningType", "TicketType" },
                unique: true);
        }
    }
}
