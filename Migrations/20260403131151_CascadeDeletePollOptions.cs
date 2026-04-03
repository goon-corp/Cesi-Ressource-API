using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ressource_API.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeletePollOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "poll_vote_poll_option_id_fk",
                table: "poll_vote");

            migrationBuilder.DropForeignKey(
                name: "polls_options_poll_id_fk",
                table: "polls_options");

            migrationBuilder.AddForeignKey(
                name: "poll_vote_poll_option_id_fk",
                table: "poll_vote",
                column: "poll_option_id",
                principalTable: "polls_options",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "polls_options_poll_id_fk",
                table: "polls_options",
                column: "poll_id",
                principalTable: "polls",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "poll_vote_poll_option_id_fk",
                table: "poll_vote");

            migrationBuilder.DropForeignKey(
                name: "polls_options_poll_id_fk",
                table: "polls_options");

            migrationBuilder.AddForeignKey(
                name: "poll_vote_poll_option_id_fk",
                table: "poll_vote",
                column: "poll_option_id",
                principalTable: "polls_options",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "polls_options_poll_id_fk",
                table: "polls_options",
                column: "poll_id",
                principalTable: "polls",
                principalColumn: "id");
        }
    }
}
