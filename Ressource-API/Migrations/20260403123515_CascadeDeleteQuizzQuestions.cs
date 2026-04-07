using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ressource_API.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteQuizzQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "quizzes_questions_quizz_id_fk",
                table: "quizzes_questions");

            migrationBuilder.AddForeignKey(
                name: "quizzes_questions_quizz_id_fk",
                table: "quizzes_questions",
                column: "quizz_id",
                principalTable: "quizzes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "quizzes_questions_quizz_id_fk",
                table: "quizzes_questions");

            migrationBuilder.AddForeignKey(
                name: "quizzes_questions_quizz_id_fk",
                table: "quizzes_questions",
                column: "quizz_id",
                principalTable: "quizzes",
                principalColumn: "id");
        }
    }
}
