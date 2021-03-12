using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class UniqueFieldinEmployeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "EmployeeInfo");

            migrationBuilder.RenameColumn(
                name: "hasParent",
                table: "Organization",
                newName: "HasParent");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "EmployeeInfo",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "EmployeeInfo",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInfo_Email",
                table: "EmployeeInfo",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInfo_PhoneNumber",
                table: "EmployeeInfo",
                column: "PhoneNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeInfo_Email",
                table: "EmployeeInfo");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeInfo_PhoneNumber",
                table: "EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "EmployeeInfo");

            migrationBuilder.RenameColumn(
                name: "HasParent",
                table: "Organization",
                newName: "hasParent");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "EmployeeInfo",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "EmployeeInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "DeletedBy", "DeletedDate", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("cc086577-d584-404a-bb5c-b44166199b01"), "351f11c6-96a0-45ee-9cc5-9cf5f6a4112a", null, null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("92dea008-9d4b-4c59-904d-7f5a700e67ae"), "3d8ba650-a301-4b21-a6e7-4940d683b9cb", null, null, "Admin", "ADMIN" },
                    { new Guid("31bfdef9-6776-4156-b727-5e8ff2a12573"), "707c9457-8067-496b-90ed-646e504e68ac", null, null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "IsSuperAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "State", "TwoFactorEnabled", "UpdatedBy", "UpdatedDate", "UserName", "ZipCode" },
                values: new object[] { new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e"), 0, null, null, "4a2237a5-9150-4c0d-9fdb-6495aa3f31b0", new Guid("cbdd9cce-707f-4fce-9066-cf9ad78307cf"), new DateTime(2021, 3, 2, 11, 2, 24, 317, DateTimeKind.Local).AddTicks(3086), null, null, "merolook@outlook.com", true, "", false, false, "", false, null, null, "SUPERADMIN", "AQAAAAEAACcQAAAAEAyhJ/c4Ro6GQpX27D9NJPwGnS5JiyLaYyCt+IpJQxfLbIV+yqFXIm/0GxuiWGpUTg==", null, false, null, null, false, null, new DateTime(2021, 3, 2, 11, 2, 24, 284, DateTimeKind.Local).AddTicks(8175), "superadmin", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("cc086577-d584-404a-bb5c-b44166199b01"), new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e") });
        }
    }
}
