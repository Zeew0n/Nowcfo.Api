using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nowcfo.Infrastructure.Migrations
{
    public partial class AddedMarketAllocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllocationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllocationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CogsType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CogsType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    PayPeriod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AllocationTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_MarketMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketAllocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarketId = table.Column<int>(type: "int", nullable: false),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    COGS = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CogsTypeId = table.Column<int>(type: "int", nullable: false),
                    SE = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    EE = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    GA = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Other = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OtherTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_MarketAllocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketAllocation_MarketMaster_MasterId",
                        column: x => x.MasterId,
                        principalTable: "MarketMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketAllocation_MasterId",
                table: "MarketAllocation",
                column: "MasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllocationType");

            migrationBuilder.DropTable(
                name: "CogsType");

            migrationBuilder.DropTable(
                name: "MarketAllocation");

            migrationBuilder.DropTable(
                name: "OtherType");

            migrationBuilder.DropTable(
                name: "MarketMaster");
        }
    }
}
