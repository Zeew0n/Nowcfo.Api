using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class AddedEmployeePermissionPagetoList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeOrgPermissions");

            migrationBuilder.CreateTable(
                name: "EmployeePermissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LevelOne = table.Column<int>(type: "int", nullable: false),
                    LevelTwo = table.Column<int>(type: "int", nullable: false),
                    LevelThree = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePermissions", x => x.PermissionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeePermissions");

            migrationBuilder.CreateTable(
                name: "EmployeeOrgPermissions",
                columns: table => new
                {
                    EmployeeOrganizationPermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Employee_Id = table.Column<int>(type: "int", nullable: true),
                    Organization_Id = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOrgPermissions", x => x.EmployeeOrganizationPermissionId);
                });
        }
    }
}
