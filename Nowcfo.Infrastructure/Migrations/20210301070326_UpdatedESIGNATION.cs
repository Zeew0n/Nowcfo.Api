using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class UpdatedESIGNATION : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("31bfdef9-6776-4156-b727-5e8ff2a12573"),
                column: "ConcurrencyStamp",
                value: "22ad3411-c065-496f-86a2-c1485740d48c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("92dea008-9d4b-4c59-904d-7f5a700e67ae"),
                column: "ConcurrencyStamp",
                value: "7c68a0e4-ff87-42d5-9f13-493ebc9edcf1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cc086577-d584-404a-bb5c-b44166199b01"),
                column: "ConcurrencyStamp",
                value: "f86f4fe5-5f26-44e4-adf9-a1e52b95d4ba");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { "22bd0f33-f5b9-4349-937c-19836af87277", new Guid("cb3721f0-8265-494c-95f7-b89affcf3d1b"), new DateTime(2021, 3, 1, 12, 45, 4, 243, DateTimeKind.Local).AddTicks(9456), "AQAAAAEAACcQAAAAEK2eHyMVWzcUWKmDOV5qlDt8lK2tUzv6RtNOi8EPdNFYlMoIfHM6uogFBp7cpJ78YA==", new DateTime(2021, 3, 1, 12, 45, 4, 234, DateTimeKind.Local).AddTicks(2061) });
        }
    }
}
