using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class hasParent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsHeadOrganization",
                table: "Organization",
                newName: "hasParent");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("31bfdef9-6776-4156-b727-5e8ff2a12573"),
                column: "ConcurrencyStamp",
                value: "707c9457-8067-496b-90ed-646e504e68ac");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("92dea008-9d4b-4c59-904d-7f5a700e67ae"),
                column: "ConcurrencyStamp",
                value: "3d8ba650-a301-4b21-a6e7-4940d683b9cb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cc086577-d584-404a-bb5c-b44166199b01"),
                column: "ConcurrencyStamp",
                value: "fd60afec-6aa7-4122-ad6a-b4d6224e4b15");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { "4a2237a5-9150-4c0d-9fdb-6495aa3f31b0", new Guid("cbdd9cce-707f-4fce-9066-cf9ad78307cf"), new DateTime(2021, 3, 2, 11, 2, 24, 317, DateTimeKind.Local).AddTicks(3086), "AQAAAAEAACcQAAAAEAyhJ/c4Ro6GQpX27D9NJPwGnS5JiyLaYyCt+IpJQxfLbIV+yqFXIm/0GxuiWGpUTg==", new DateTime(2021, 3, 2, 11, 2, 24, 284, DateTimeKind.Local).AddTicks(8175) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "hasParent",
                table: "Organization",
                newName: "IsHeadOrganization");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("31bfdef9-6776-4156-b727-5e8ff2a12573"),
                column: "ConcurrencyStamp",
                value: "2bc4d374-ac88-41eb-aa97-2c1438f4366a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("92dea008-9d4b-4c59-904d-7f5a700e67ae"),
                column: "ConcurrencyStamp",
                value: "54c64259-8485-4e34-8b41-1e7553ef036b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cc086577-d584-404a-bb5c-b44166199b01"),
                column: "ConcurrencyStamp",
                value: "9970089c-eae9-4789-b57c-d9d99c99ea82");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b3bb50ef-d624-41de-a93b-2031d0fd392e"),
                columns: new[] { "ConcurrencyStamp", "CreatedBy", "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { "6e4478dc-72eb-43c0-8e3d-acfdcbad096e", new Guid("29bf6c1c-06aa-420d-a80e-d191f0319294"), new DateTime(2021, 3, 2, 10, 16, 26, 375, DateTimeKind.Local).AddTicks(3906), "AQAAAAEAACcQAAAAEMSngnPvOF9u+phlsvM+ERyDzbPijAZUM56JgTiL5WmPvHAOjzQBwftWZQVumTBFRw==", new DateTime(2021, 3, 2, 10, 16, 26, 342, DateTimeKind.Local).AddTicks(9488) });
        }
    }
}
