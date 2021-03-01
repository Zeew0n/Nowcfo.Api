using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class UpdateOrgRolesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Designation",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Email", "PasswordHash", "UpdatedDate" },
                values: new object[] { "22bd0f33-f5b9-4349-937c-19836af87277", new Guid("cb3721f0-8265-494c-95f7-b89affcf3d1b"), new DateTime(2021, 3, 1, 12, 45, 4, 243, DateTimeKind.Local).AddTicks(9456), "merolook@outlook.com", "AQAAAAEAACcQAAAAEK2eHyMVWzcUWKmDOV5qlDt8lK2tUzv6RtNOi8EPdNFYlMoIfHM6uogFBp7cpJ78YA==", new DateTime(2021, 3, 1, 12, 45, 4, 234, DateTimeKind.Local).AddTicks(2061) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Designation");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("31bfdef9-6776-4156-b727-5e8ff2a12573"),
                column: "ConcurrencyStamp",
                value: "2421b5e4-7c77-4a85-b96a-c8166dd90d60");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("92dea008-9d4b-4c59-904d-7f5a700e67ae"),
                column: "ConcurrencyStamp",
                value: "3dbc1782-1fe9-4c65-a6f3-0923af71f268");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cc086577-d584-404a-bb5c-b44166199b01"),
                column: "ConcurrencyStamp",
                value: "56649344-11b8-4a4c-9390-ec87956e2d8d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "Email", "PasswordHash", "UpdatedDate" },
                values: new object[] { "9c5a2bca-d701-46e6-9502-ba0441425279", new Guid("2b074da2-e6e5-425e-a550-eec3262aac49"), new DateTime(2021, 2, 24, 11, 20, 28, 989, DateTimeKind.Local).AddTicks(9185), null, "AQAAAAEAACcQAAAAENSaTTF2N+tuorptzYhS/hiNwfaKU+iy4PU6wOaNHMWNwr0zB+sDpKIrfwOkjrLMkg==", new DateTime(2021, 2, 24, 11, 20, 28, 958, DateTimeKind.Local).AddTicks(4859) });
        }
    }
}
