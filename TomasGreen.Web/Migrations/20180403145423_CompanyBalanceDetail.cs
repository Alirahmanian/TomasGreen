using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class CompanyBalanceDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyBalanceDetailTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBalanceDetailTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyBalanceDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    BalanceChangerID = table.Column<int>(nullable: false),
                    CompanyBalanceDetailTypeID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CurrencyID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBalanceDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CompanyBalanceDetails_CompanyBalanceDetailTypes_CompanyBalanceDetailTypeID",
                        column: x => x.CompanyBalanceDetailTypeID,
                        principalTable: "CompanyBalanceDetailTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetails_CompanyBalanceDetailTypeID",
                table: "CompanyBalanceDetails",
                column: "CompanyBalanceDetailTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetails_CompanyID_CurrencyID_CompanyBalanceDetailTypeID_BalanceChangerID",
                table: "CompanyBalanceDetails",
                columns: new[] { "CompanyID", "CurrencyID", "CompanyBalanceDetailTypeID", "BalanceChangerID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetailTypes_Name",
                table: "CompanyBalanceDetailTypes",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyBalanceDetails");

            migrationBuilder.DropTable(
                name: "CompanyBalanceDetailTypes");
        }
    }
}
