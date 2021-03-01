using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class UpdatedIsActiveEmployeeClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeInfo");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("31bfdef9-6776-4156-b727-5e8ff2a12573"),
                column: "ConcurrencyStamp",
                value: "ce4ce8dc-692b-4e40-8331-aceefda0be68");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("92dea008-9d4b-4c59-904d-7f5a700e67ae"),
                column: "ConcurrencyStamp",
                value: "bb2439bd-4ef8-41cb-ba78-672bd9e4bd43");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cc086577-d584-404a-bb5c-b44166199b01"),
                column: "ConcurrencyStamp",
                value: "b5edbfdd-c686-4d0a-bac4-35876ce83915");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { "bb9e2619-eed0-4e4c-8fa7-6e506c14a7c6", new Guid("46eb92df-a690-42aa-be8a-b66da2109943"), new DateTime(2021, 3, 1, 12, 48, 25, 916, DateTimeKind.Local).AddTicks(9053), "AQAAAAEAACcQAAAAELeCYtVsVAGHFXQrfHc7TxM0v/I0EcdWa4WEMFzoXS+S/hgw+/QpYiWm/xrq+pIPHQ==", new DateTime(2021, 3, 1, 12, 48, 25, 909, DateTimeKind.Local).AddTicks(22) });
        }
    }
}
