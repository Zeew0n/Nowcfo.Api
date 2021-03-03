using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class Add_EmpOrgPermisson_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeOrgPermissions",
                columns: table => new
                {
                    EmployeeOrganizationPermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee_Id = table.Column<int>(type: "int", nullable: true),
                    Organization_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOrgPermissions", x => x.EmployeeOrganizationPermissionId);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("31bfdef9-6776-4156-b727-5e8ff2a12573"),
                column: "ConcurrencyStamp",
                value: "55bc9095-7961-4e96-adb1-cbcac65d73e5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("92dea008-9d4b-4c59-904d-7f5a700e67ae"),
                column: "ConcurrencyStamp",
                value: "ba106fbe-a01c-4289-81ab-45c825fb2c54");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cc086577-d584-404a-bb5c-b44166199b01"),
                column: "ConcurrencyStamp",
                value: "351f11c6-96a0-45ee-9cc5-9cf5f6a4112a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { "c50382e0-9adf-4cdc-83cc-ec4fa51155b4", new Guid("5177b80e-ae95-4d11-99e0-20d878e9809b"), new DateTime(2021, 3, 3, 11, 34, 21, 943, DateTimeKind.Local).AddTicks(9725), "AQAAAAEAACcQAAAAECguE8pRg+HF4XencnZrBUltr9QTHWXYe7utC0uQDxlA/YudWUR+bmwLWcJgj596kA==", new DateTime(2021, 3, 3, 11, 34, 21, 935, DateTimeKind.Local).AddTicks(1389) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeOrgPermissions");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("31bfdef9-6776-4156-b727-5e8ff2a12573"),
                column: "ConcurrencyStamp",
                value: "b1188854-d8bc-4407-935e-f3af3e1df2fc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("92dea008-9d4b-4c59-904d-7f5a700e67ae"),
                column: "ConcurrencyStamp",
                value: "2580d284-76fd-4ca6-a641-8a546428a973");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cc086577-d584-404a-bb5c-b44166199b01"),
                column: "ConcurrencyStamp",
                value: "4503eac0-6642-4e03-a5c8-e068f57a3f1f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { "578b52fb-ac12-405b-ac72-b75ce96a749d", new Guid("f2e965e9-70e7-4eac-9e21-4f19f7c3299f"), new DateTime(2021, 3, 1, 16, 5, 34, 244, DateTimeKind.Local).AddTicks(6743), "AQAAAAEAACcQAAAAEPGr8k4pvUU/yUaQOq4im7due8aOHJGqpnftBJVK8LqpBCS6iapVEpI5eYbPO83lMg==", new DateTime(2021, 3, 1, 16, 5, 34, 236, DateTimeKind.Local).AddTicks(6778) });
        }
    }
}
