using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class AddedStartDateandTerminationDateEmployeeModal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "EmployeeInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TerminationDate",
                table: "EmployeeInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "TerminationDate",
                table: "EmployeeInfo");
        }
    }
}
