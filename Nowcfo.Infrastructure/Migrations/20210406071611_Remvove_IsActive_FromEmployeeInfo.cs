using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class Remvove_IsActive_FromEmployeeInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
