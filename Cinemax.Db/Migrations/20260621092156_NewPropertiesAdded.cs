using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemax.Db.Migrations
{
    /// <inheritdoc />
    public partial class NewPropertiesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TicketPrices",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Screenings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Scheduled");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Genres",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CinemaHalls",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TicketPrices");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Screenings");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CinemaHalls");
        }
    }
}
