using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ressource_API.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenExpirationTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "expiration_time",
                table: "refresh_tokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expiration_time",
                table: "refresh_tokens");
        }
    }
}
