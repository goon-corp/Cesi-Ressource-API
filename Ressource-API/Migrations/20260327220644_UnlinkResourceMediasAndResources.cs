using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ressource_API.Migrations
{
    /// <inheritdoc />
    public partial class UnlinkResourceMediasAndResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "ressources_medias_ressource_id_fk",
                table: "ressources_medias");

            migrationBuilder.DropColumn(
                name: "ressource_id",
                table: "ressources_medias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ressource_id",
                table: "ressources_medias",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "ressources_medias_ressource_id_fk",
                table: "ressources_medias",
                column: "ressource_id",
                principalTable: "ressources",
                principalColumn: "id");
        }
    }
}
