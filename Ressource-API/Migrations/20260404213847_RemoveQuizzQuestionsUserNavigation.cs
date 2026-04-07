using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ressource_API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveQuizzQuestionsUserNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "QuizzQuestionUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "QuizzQuestionUser",
            //     columns: table => new
            //     {
            //         QuizzQuestionsId = table.Column<Guid>(type: "uuid", nullable: false),
            //         UsersId = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_QuizzQuestionUser", x => new { x.QuizzQuestionsId, x.UsersId });
            //         table.ForeignKey(
            //             name: "FK_QuizzQuestionUser_quizzes_questions_QuizzQuestionsId",
            //             column: x => x.QuizzQuestionsId,
            //             principalTable: "quizzes_questions",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_QuizzQuestionUser_users_UsersId",
            //             column: x => x.UsersId,
            //             principalTable: "users",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Cascade);
            //     });
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_QuizzQuestionUser_UsersId",
            //     table: "QuizzQuestionUser",
            //     column: "UsersId");
        }
    }
}
