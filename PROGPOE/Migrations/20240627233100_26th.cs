using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROGPOE.Migrations
{
    /// <inheritdoc />
    public partial class _26th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mobile",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mobile",
                table: "Employees",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
