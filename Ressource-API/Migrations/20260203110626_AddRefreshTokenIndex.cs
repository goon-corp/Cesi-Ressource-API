using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ressource_API.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "token_hash_IDX",
                table: "refresh_tokens",
                column: "refresh_token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "token_hash_IDX",
                table: "refresh_tokens");
        }
    }
}
