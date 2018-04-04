using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class renmaingCompanyBalanceDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyBalanceDetails");

            migrationBuilder.DropTable(
                name: "CompanyBalanceDetailTypes");

            migrationBuilder.CreateTable(
                name: "CompanyCreditDebitBalanceDetailTypes",
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
                    table.PrimaryKey("PK_CompanyCreditDebitBalanceDetailTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyCreditDebitBalanceDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    BalanceChangerID = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    CompanyCreditDebitBalanceDetailTypeID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    CreditBeforeChange = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CurrencyID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    DebitBeforeChange = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PaymentTypeID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyCreditDebitBalanceDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CompanyCreditDebitBalanceDetails_CompanyCreditDebitBalanceDetailTypes_CompanyCreditDebitBalanceDetailTypeID",
                        column: x => x.CompanyCreditDebitBalanceDetailTypeID,
                        principalTable: "CompanyCreditDebitBalanceDetailTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyCreditDebitBalanceDetails_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyCreditDebitBalanceDetails_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyCreditDebitBalanceDetails_PaymentTypes_PaymentTypeID",
                        column: x => x.PaymentTypeID,
                        principalTable: "PaymentTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCreditDebitBalanceDetails_CompanyCreditDebitBalanceDetailTypeID",
                table: "CompanyCreditDebitBalanceDetails",
                column: "CompanyCreditDebitBalanceDetailTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCreditDebitBalanceDetails_CurrencyID",
                table: "CompanyCreditDebitBalanceDetails",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCreditDebitBalanceDetails_PaymentTypeID",
                table: "CompanyCreditDebitBalanceDetails",
                column: "PaymentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCreditDebitBalanceDetails_CompanyID_CurrencyID_CompanyCreditDebitBalanceDetailTypeID_BalanceChangerID_PaymentTypeID",
                table: "CompanyCreditDebitBalanceDetails",
                columns: new[] { "CompanyID", "CurrencyID", "CompanyCreditDebitBalanceDetailTypeID", "BalanceChangerID", "PaymentTypeID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCreditDebitBalanceDetailTypes_Name",
                table: "CompanyCreditDebitBalanceDetailTypes",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyCreditDebitBalanceDetails");

            migrationBuilder.DropTable(
                name: "CompanyCreditDebitBalanceDetailTypes");

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
                    Comment = table.Column<string>(nullable: true),
                    CompanyBalanceDetailTypeID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CreditBeforeChange = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CurrencyID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DebitBeforeChange = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PaymentTypeID = table.Column<int>(nullable: false),
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
                    table.ForeignKey(
                        name: "FK_CompanyBalanceDetails_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyBalanceDetails_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyBalanceDetails_PaymentTypes_PaymentTypeID",
                        column: x => x.PaymentTypeID,
                        principalTable: "PaymentTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetails_CompanyBalanceDetailTypeID",
                table: "CompanyBalanceDetails",
                column: "CompanyBalanceDetailTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetails_CurrencyID",
                table: "CompanyBalanceDetails",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetails_PaymentTypeID",
                table: "CompanyBalanceDetails",
                column: "PaymentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetails_CompanyID_CurrencyID_CompanyBalanceDetailTypeID_BalanceChangerID_PaymentTypeID",
                table: "CompanyBalanceDetails",
                columns: new[] { "CompanyID", "CurrencyID", "CompanyBalanceDetailTypeID", "BalanceChangerID", "PaymentTypeID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetailTypes_Name",
                table: "CompanyBalanceDetailTypes",
                column: "Name",
                unique: true);
        }
    }
}
