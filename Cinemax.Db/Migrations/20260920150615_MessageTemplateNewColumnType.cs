using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemax.Db.Migrations
{
    /// <inheritdoc />
    public partial class MessageTemplateNewColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "MessageTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "MessageTemplates");
        }
    }
}
