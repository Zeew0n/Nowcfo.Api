using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class AddedBaseEntityinEmployeePermissionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "EmployeePermissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EmployeePermissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "EmployeePermissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "EmployeePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployeePermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "EmployeePermissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EmployeePermissions",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeePermissions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EmployeePermissions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EmployeePermissions");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "EmployeePermissions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmployeePermissions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EmployeePermissions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EmployeePermissions");
        }
    }
}
