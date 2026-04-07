using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ressource_API.Migrations
{
    /// <inheritdoc />
    public partial class RemovePasswordHistorySalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password_salt",
                table: "passwords_history");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password_salt",
                table: "passwords_history",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
