using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Migrations
{
    /// <inheritdoc />
    public partial class ddssqds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StartedQuizStudents_StartedQuizTeachers_StartedQuizTeacherIdStartedQuizTeacher",
                table: "StartedQuizStudents");

            migrationBuilder.DropIndex(
                name: "IX_StartedQuizStudents_StartedQuizTeacherIdStartedQuizTeacher",
                table: "StartedQuizStudents");

            migrationBuilder.DropColumn(
                name: "StartedQuizTeacherIdStartedQuizTeacher",
                table: "StartedQuizStudents");

            migrationBuilder.AlterColumn<int>(
                name: "IdStartedQuizTeacher",
                table: "StartedQuizStudents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StartedQuizStudents_IdStartedQuizTeacher",
                table: "StartedQuizStudents",
                column: "IdStartedQuizTeacher");

            migrationBuilder.AddForeignKey(
                name: "FK_StartedQuizStudents_StartedQuizTeachers_IdStartedQuizTeacher",
                table: "StartedQuizStudents",
                column: "IdStartedQuizTeacher",
                principalTable: "StartedQuizTeachers",
                principalColumn: "IdStartedQuizTeacher",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StartedQuizStudents_StartedQuizTeachers_IdStartedQuizTeacher",
                table: "StartedQuizStudents");

            migrationBuilder.DropIndex(
                name: "IX_StartedQuizStudents_IdStartedQuizTeacher",
                table: "StartedQuizStudents");

            migrationBuilder.AlterColumn<int>(
                name: "IdStartedQuizTeacher",
                table: "StartedQuizStudents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StartedQuizTeacherIdStartedQuizTeacher",
                table: "StartedQuizStudents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StartedQuizStudents_StartedQuizTeacherIdStartedQuizTeacher",
                table: "StartedQuizStudents",
                column: "StartedQuizTeacherIdStartedQuizTeacher");

            migrationBuilder.AddForeignKey(
                name: "FK_StartedQuizStudents_StartedQuizTeachers_StartedQuizTeacherIdStartedQuizTeacher",
                table: "StartedQuizStudents",
                column: "StartedQuizTeacherIdStartedQuizTeacher",
                principalTable: "StartedQuizTeachers",
                principalColumn: "IdStartedQuizTeacher");
        }
    }
}
