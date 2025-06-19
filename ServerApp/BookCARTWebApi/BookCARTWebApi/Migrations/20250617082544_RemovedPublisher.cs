using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookCARTWebApi.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPublisher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
