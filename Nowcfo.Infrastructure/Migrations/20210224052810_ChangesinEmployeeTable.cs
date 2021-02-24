using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class ChangesinEmployeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "EmployeeInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "EmployeeInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "EmployeeInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OverTimeRate",
                table: "EmployeeInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pay",
                table: "EmployeeInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PayType",
                table: "EmployeeInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "EmployeeInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "EmployeeInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "City",
                table: "EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "OverTimeRate",
                table: "EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "Pay",
                table: "EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "PayType",
                table: "EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "EmployeeInfo");
        }
    }
}
