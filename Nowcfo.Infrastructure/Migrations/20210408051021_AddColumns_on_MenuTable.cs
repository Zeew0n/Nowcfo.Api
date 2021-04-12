using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class AddColumns_on_MenuTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuLevel",
                table: "Menu",
                type: "int",
                nullable: true,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "NavigateUrl",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UnderMenuId",
                table: "Menu",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuLevel",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "NavigateUrl",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "UnderMenuId",
                table: "Menu");


            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployeeOrgPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
