using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StartedQuizTeachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: true),
                    CodeQuiz = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsStarted = table.Column<bool>(type: "bit", nullable: false),
                    IsTerminated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartedQuizTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StartedQuizTeachers_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId");
                    table.ForeignKey(
                        name: "FK_StartedQuizTeachers_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartedQuizStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    StartedQuizTeacherId = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartedQuizStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StartedQuizStudents_StartedQuizTeachers_StartedQuizTeacherId",
                        column: x => x.StartedQuizTeacherId,
                        principalTable: "StartedQuizTeachers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StartedQuizStudents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StartedQuizStudents_StartedQuizTeacherId",
                table: "StartedQuizStudents",
                column: "StartedQuizTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StartedQuizStudents_UserId",
                table: "StartedQuizStudents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StartedQuizTeachers_QuizId",
                table: "StartedQuizTeachers",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_StartedQuizTeachers_TeacherId",
                table: "StartedQuizTeachers",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StartedQuizStudents");

            migrationBuilder.DropTable(
                name: "StartedQuizTeachers");
        }
    }
}
