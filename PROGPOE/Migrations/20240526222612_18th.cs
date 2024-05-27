using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROGPOE.Migrations
{
    /// <inheritdoc />
    public partial class _18th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
name: "Farmers",
columns: table => new
{
    FarmerID = table.Column<int>(type: "int", nullable: false)
.Annotation("SqlServer:Identity", "1, 1"),
    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
    EmployeeID = table.Column<int>(type: "int", nullable: false)
},
constraints: table =>
{
    table.PrimaryKey("PK_Farmers", x => x.FarmerID);
    table.ForeignKey(
    name: "FK_Farmers_Employees_EmployeeID",
    column: x => x.EmployeeID,
    principalTable: "Employees",
    principalColumn: "EmployeeID",
    onDelete: ReferentialAction.Cascade);
});
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
    name: "Farmers");
        
    }
    }
}
