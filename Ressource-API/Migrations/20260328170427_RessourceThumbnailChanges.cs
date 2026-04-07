using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ressource_API.Migrations
{
    /// <inheritdoc />
    public partial class RessourceThumbnailChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "thumbnail_url",
                table: "ressources");

            migrationBuilder.AddColumn<Guid>(
                name: "thumbnail_id",
                table: "ressources",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ressources_thumbnail_id",
                table: "ressources",
                column: "thumbnail_id");

            migrationBuilder.AddForeignKey(
                name: "ressources_thumbnail_id_fk",
                table: "ressources",
                column: "thumbnail_id",
                principalTable: "ressources_medias",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ressources_thumbnail_id_fk",
                table: "ressources");

            migrationBuilder.DropIndex(
                name: "IX_ressources_thumbnail_id",
                table: "ressources");

            migrationBuilder.DropColumn(
                name: "thumbnail_id",
                table: "ressources");

            migrationBuilder.AddColumn<string>(
                name: "thumbnail_url",
                table: "ressources",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
