using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemax.Db.Migrations
{
    /// <inheritdoc />
    public partial class NewEntityEmailNotificationsResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailNotificationsResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailNotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNotificationsResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailNotificationsResources_EmailNotifications_EmailNotificationId",
                        column: x => x.EmailNotificationId,
                        principalTable: "EmailNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailNotificationsResources_EmailNotificationId",
                table: "EmailNotificationsResources",
                column: "EmailNotificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailNotificationsResources");
        }
    }
}
