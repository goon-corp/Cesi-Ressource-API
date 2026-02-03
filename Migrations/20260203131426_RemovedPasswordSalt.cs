using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ressource_API.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPasswordSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password_salt",
                table: "logins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password_salt",
                table: "logins",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
