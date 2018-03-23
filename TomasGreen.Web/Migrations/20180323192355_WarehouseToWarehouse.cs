using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class WarehouseToWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarehouseToWarehouses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    RecipientCompanyID = table.Column<int>(nullable: false),
                    RecipientWarehouseID = table.Column<int>(nullable: false),
                    SenderCompanyID = table.Column<int>(nullable: false),
                    SenderWarehouseID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseToWarehouses", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseToWarehouses_Date_SenderWarehouseID_RecipientWarehouseID_ArticleID",
                table: "WarehouseToWarehouses",
                columns: new[] { "Date", "SenderWarehouseID", "RecipientWarehouseID", "ArticleID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseToWarehouses");
        }
    }
}
