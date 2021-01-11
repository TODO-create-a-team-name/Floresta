using Microsoft.EntityFrameworkCore.Migrations;

namespace Floresta.Migrations
{
    public partial class AddedQuestionTopictable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionTopicId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestionTopics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTopics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTopicId",
                table: "Questions",
                column: "QuestionTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionTopics_QuestionTopicId",
                table: "Questions",
                column: "QuestionTopicId",
                principalTable: "QuestionTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionTopics_QuestionTopicId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionTopics");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionTopicId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionTopicId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Questions");
        }
    }
}
