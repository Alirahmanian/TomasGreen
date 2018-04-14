using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class AddingCashPropertyToCompanyToCompanyPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyToCompanyPayments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    Cash = table.Column<bool>(nullable: false),
                    CurrencyID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    ExhangedAmount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Guid = table.Column<Guid>(nullable: true),
                    IsDiscountRate = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    PaidCurrencyID = table.Column<int>(nullable: false),
                    PayingCompanyID = table.Column<int>(nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    ReceivingCompanyID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyToCompanyPayments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CompanyToCompanyPayments_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyToCompanyPayments_Currencies_PaidCurrencyID",
                        column: x => x.PaidCurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyToCompanyPayments_Companies_PayingCompanyID",
                        column: x => x.PayingCompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyToCompanyPayments_Companies_ReceivingCompanyID",
                        column: x => x.ReceivingCompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyToCompanyPayments_CurrencyID",
                table: "CompanyToCompanyPayments",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyToCompanyPayments_PaidCurrencyID",
                table: "CompanyToCompanyPayments",
                column: "PaidCurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyToCompanyPayments_PayingCompanyID",
                table: "CompanyToCompanyPayments",
                column: "PayingCompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyToCompanyPayments_ReceivingCompanyID",
                table: "CompanyToCompanyPayments",
                column: "ReceivingCompanyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyToCompanyPayments");
        }
    }
}
