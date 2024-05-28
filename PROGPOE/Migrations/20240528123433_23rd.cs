using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROGPOE.Migrations
{
    /// <inheritdoc />
    public partial class _23rd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "Farmers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "Farmers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
