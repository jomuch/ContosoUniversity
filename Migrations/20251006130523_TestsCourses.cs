using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContosoUniversity.Migrations
{
    /// <inheritdoc />
    public partial class TestsCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstMidName",
                table: "Students",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "FirstMidName",
                table: "Instructors",
                newName: "FirstName");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "OfficeAssignments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Students",
                newName: "FirstMidName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Instructors",
                newName: "FirstMidName");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "OfficeAssignments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
