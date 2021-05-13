using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class SalesForecast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesForecast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayPeriod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BillRate = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BillHours = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Placements = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BuyOuts = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    EstimatedRevenue = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Cogs = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CogsQkly = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ClosedPayPeriods = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OtherPercent = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesForecast", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesForecast_PayPeriod",
                table: "SalesForecast",
                column: "PayPeriod",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesForecast");
        }
    }
}
