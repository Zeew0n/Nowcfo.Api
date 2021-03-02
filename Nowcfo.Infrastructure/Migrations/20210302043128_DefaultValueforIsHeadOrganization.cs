using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class DefaultValueforIsHeadOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsHeadOrganization",
                table: "Organization",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsHeadOrganization",
                table: "Organization",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

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
