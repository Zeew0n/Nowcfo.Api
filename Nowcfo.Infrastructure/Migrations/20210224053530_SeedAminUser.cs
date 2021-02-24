using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class SeedAminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "DeletedBy", "DeletedDate", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("cc086577-d584-404a-bb5c-b44166199b01"), "56649344-11b8-4a4c-9390-ec87956e2d8d", null, null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("92dea008-9d4b-4c59-904d-7f5a700e67ae"), "3dbc1782-1fe9-4c65-a6f3-0923af71f268", null, null, "Admin", "ADMIN" },
                    { new Guid("31bfdef9-6776-4156-b727-5e8ff2a12573"), "2421b5e4-7c77-4a85-b96a-c8166dd90d60", null, null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "IsSuperAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "State", "TwoFactorEnabled", "UpdatedBy", "UpdatedDate", "UserName", "ZipCode" },
                values: new object[] { new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e"), 0, null, null, "9c5a2bca-d701-46e6-9502-ba0441425279", new Guid("2b074da2-e6e5-425e-a550-eec3262aac49"), new DateTime(2021, 2, 24, 11, 20, 28, 989, DateTimeKind.Local).AddTicks(9185), null, null, "merolook@outlook.com", true, "", false, false, "", false, null, null, "SUPERADMIN", "AQAAAAEAACcQAAAAENSaTTF2N+tuorptzYhS/hiNwfaKU+iy4PU6wOaNHMWNwr0zB+sDpKIrfwOkjrLMkg==", null, false, null, null, false, null, new DateTime(2021, 2, 24, 11, 20, 28, 958, DateTimeKind.Local).AddTicks(4859), "superadmin", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("cc086577-d584-404a-bb5c-b44166199b01"), new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("31bfdef9-6776-4156-b727-5e8ff2a12573"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("92dea008-9d4b-4c59-904d-7f5a700e67ae"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("cc086577-d584-404a-bb5c-b44166199b01"), new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cc086577-d584-404a-bb5c-b44166199b01"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e"));
        }
    }
}
