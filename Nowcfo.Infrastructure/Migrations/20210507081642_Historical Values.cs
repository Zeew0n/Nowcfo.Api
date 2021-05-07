using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class HistoricalValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompensationHistorical",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Pay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OverTimeRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompensationHistorical", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeStatusHistorical",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeStatusHistorical", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobRoleHistorical",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRoleHistorical", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayTypeHistorical",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PayType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayTypeHistorical", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePermissions_EmployeeId_LevelOne_LevelTwo_LevelThree_ReferenceId",
                table: "EmployeePermissions",
                columns: new[] { "EmployeeId", "LevelOne", "LevelTwo", "LevelThree", "ReferenceId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompensationHistorical");

            migrationBuilder.DropTable(
                name: "EmployeeStatusHistorical");

            migrationBuilder.DropTable(
                name: "JobRoleHistorical");

            migrationBuilder.DropTable(
                name: "PayTypeHistorical");

            migrationBuilder.DropIndex(
                name: "IX_EmployeePermissions_EmployeeId_LevelOne_LevelTwo_LevelThree_ReferenceId",
                table: "EmployeePermissions");
        }
    }
}
