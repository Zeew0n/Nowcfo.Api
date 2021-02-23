using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "DeletedBy", "DeletedDate", "Name", "NormalizedName" },
                values: new object[] { new Guid("be70cd03-8c42-4592-baa3-5726de19d922"), "cdce82a0-c51e-4031-9b7d-1146275d36e2", null, null, "SuperAdmin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "DeletedBy", "DeletedDate", "Name", "NormalizedName" },
                values: new object[] { new Guid("a481f160-897d-4ae1-9c00-4fbdb523ad01"), "aa6b40ab-a480-4c3f-807e-5af913f29ab8", null, null, "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "DeletedBy", "DeletedDate", "Name", "NormalizedName" },
                values: new object[] { new Guid("61891920-cec7-45ef-af88-1eb33dfe67d2"), "525fc979-e0a7-40b4-a777-8df5da184f1c", null, null, "User", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("61891920-cec7-45ef-af88-1eb33dfe67d2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a481f160-897d-4ae1-9c00-4fbdb523ad01"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("be70cd03-8c42-4592-baa3-5726de19d922"));
        }
    }
}
