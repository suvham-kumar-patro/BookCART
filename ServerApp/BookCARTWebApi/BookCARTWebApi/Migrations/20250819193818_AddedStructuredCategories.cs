using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookCARTWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedStructuredCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Board",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CollegeLevel",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Honors",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainCategory",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalCourse",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OthersCategory",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolClass",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Board",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CollegeLevel",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Course",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Honors",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MainCategory",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MedicalCourse",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "OthersCategory",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "SchoolClass",
                table: "Books");
        }
    }
}
