using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class IntTomasGreen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleCategories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ArticlePakageForms",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlePakageForms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ArticleUnits",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    MeasuresByG = table.Column<bool>(nullable: false),
                    MeasuresByKg = table.Column<bool>(nullable: false),
                    MeasuresByTon = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleUnits", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    Balance = table.Column<decimal>(nullable: false),
                    CreditLimit = table.Column<decimal>(nullable: false),
                    CreditReceived = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    IsOwner = table.Column<bool>(nullable: false),
                    LastBalance = table.Column<decimal>(nullable: false),
                    LastBalanceDate = table.Column<DateTime>(nullable: false),
                    Locked = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Paid = table.Column<decimal>(nullable: false),
                    Purchases = table.Column<decimal>(nullable: false),
                    Received = table.Column<decimal>(nullable: false),
                    Ruble = table.Column<bool>(nullable: false),
                    SouldToUs = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Age = table.Column<int>(nullable: true),
                    Archive = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(maxLength: 255, nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderTranports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTranports", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PackagingCategory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackagingCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PackingPlans",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: true),
                    ManagerID = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingPlans", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanySections",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CompanySections_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    ArticleCategoryID = table.Column<int>(nullable: false),
                    ArticlePackageFormID = table.Column<int>(nullable: false),
                    ArticleUnitID = table.Column<int>(nullable: false),
                    CountryID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    MinimumPricePerUSD = table.Column<decimal>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WeightPerPackage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Articles_ArticleCategories_ArticleCategoryID",
                        column: x => x.ArticleCategoryID,
                        principalTable: "ArticleCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_ArticlePakageForms_ArticlePackageFormID",
                        column: x => x.ArticlePackageFormID,
                        principalTable: "ArticlePakageForms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_ArticleUnits_ArticleUnitID",
                        column: x => x.ArticleUnitID,
                        principalTable: "ArticleUnits",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Countries_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoastingPlans",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<int>(nullable: false),
                    Bags = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FromWarehouseID = table.Column<int>(nullable: false),
                    ManagerID = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    NewArticleID = table.Column<int>(nullable: false),
                    NewQtyExtra = table.Column<decimal>(nullable: false),
                    NewQtyPackages = table.Column<int>(nullable: false),
                    NewTotalWeight = table.Column<decimal>(nullable: false),
                    Packages = table.Column<int>(nullable: false),
                    PackagingMaterialBagID = table.Column<int>(nullable: false),
                    PackagingMaterialPackageID = table.Column<int>(nullable: false),
                    PricePerUnit = table.Column<decimal>(nullable: false),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    Salt = table.Column<decimal>(nullable: false),
                    SaltLimit = table.Column<decimal>(nullable: false),
                    ToWarehouseID = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    TotalWeight = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WeightChange = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoastingPlans", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoastingPlans_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoastingPlans_Employees_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    AmountArticle = table.Column<int>(nullable: false),
                    AmountPaid = table.Column<decimal>(nullable: true),
                    AmountReserve = table.Column<int>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    Cash = table.Column<bool>(nullable: false),
                    Coments = table.Column<string>(nullable: true),
                    CompanyID = table.Column<int>(nullable: false),
                    Confirmed = table.Column<bool>(nullable: false),
                    EmployeeID = table.Column<int>(nullable: false),
                    ForcedPaid = table.Column<bool>(nullable: false),
                    Guid = table.Column<Guid>(nullable: true),
                    LoadedDate = table.Column<DateTime>(nullable: true),
                    LoadingDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderPaid = table.Column<bool>(nullable: false),
                    OrderTransportID = table.Column<int>(nullable: false),
                    OrderdBy = table.Column<string>(nullable: true),
                    PaidDate = table.Column<DateTime>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    PaymentWarning = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderTranports_OrderTransportID",
                        column: x => x.OrderTransportID,
                        principalTable: "OrderTranports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PackagingMaterials",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleUnitID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Dimensions = table.Column<decimal>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PackagingCategoryID = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Volume = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackagingMaterials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PackagingMaterials_ArticleUnits_ArticleUnitID",
                        column: x => x.ArticleUnitID,
                        principalTable: "ArticleUnits",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackagingMaterials_PackagingCategory_PackagingCategoryID",
                        column: x => x.PackagingCategoryID,
                        principalTable: "PackagingCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Archive = table.Column<bool>(nullable: false),
                    CompanySectionID = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsCustomers = table.Column<bool>(nullable: false),
                    IsOnTheWay = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Warehouses_CompanySections_CompanySectionID",
                        column: x => x.CompanySectionID,
                        principalTable: "CompanySections",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedArticles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    ArticleID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: true),
                    ContainerNumber = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Discount = table.Column<decimal>(nullable: false),
                    ExpectedToArrive = table.Column<DateTime>(nullable: true),
                    Guid = table.Column<Guid>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PenaltyFee = table.Column<decimal>(nullable: false),
                    Received = table.Column<bool>(nullable: false),
                    TollFee = table.Column<decimal>(nullable: false),
                    TotalPerUnit = table.Column<decimal>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    TransportCost = table.Column<decimal>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedArticles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PurchasedArticles_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticles_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleWarehouseBalances",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    QtyExtraIn = table.Column<decimal>(nullable: false),
                    QtyExtraOnhand = table.Column<decimal>(nullable: false),
                    QtyExtraOut = table.Column<decimal>(nullable: false),
                    QtyPackagesIn = table.Column<int>(nullable: false),
                    QtyPackagesOnhand = table.Column<int>(nullable: false),
                    QtyPackagesOut = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleWarehouseBalances", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArticleWarehouseBalances_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleWarehouseBalances_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleWarehouseBalances_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicingPlans",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: true),
                    ManagerID = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    TotalWeight = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicingPlans", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DicingPlans_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicingPlans_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicingPlans_Employees_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicingPlans_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<int>(nullable: false),
                    Extended_Price = table.Column<decimal>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    OrderID = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PackingPlanMixs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Bags = table.Column<int>(nullable: false),
                    Guid = table.Column<Guid>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    NewArticleID = table.Column<int>(nullable: false),
                    NewQtyExtra = table.Column<decimal>(nullable: false),
                    NewQtyPackages = table.Column<int>(nullable: false),
                    NewTotalWeight = table.Column<decimal>(nullable: false),
                    Packages = table.Column<int>(nullable: false),
                    PackagingMaterialBagID = table.Column<int>(nullable: false),
                    PackagingMaterialPackageID = table.Column<int>(nullable: false),
                    PackingPlanID = table.Column<int>(nullable: false),
                    PricePerUnit = table.Column<decimal>(nullable: false),
                    ToWarehouseID = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingPlanMixs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PackingPlanMixs_Articles_NewArticleID",
                        column: x => x.NewArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackingPlanMixs_PackingPlans_PackingPlanID",
                        column: x => x.PackingPlanID,
                        principalTable: "PackingPlans",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackingPlanMixs_Warehouses_ToWarehouseID",
                        column: x => x.ToWarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedArticleWarehouses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArrivedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PurchasedArticleID = table.Column<int>(nullable: false),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyExtraArrived = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    QtyPackagesArrived = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedArticleWarehouses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleWarehouses_PurchasedArticles_PurchasedArticleID",
                        column: x => x.PurchasedArticleID,
                        principalTable: "PurchasedArticles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleWarehouses_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicingPlanDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<int>(nullable: false),
                    DicingPlanID = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    TotalWeight = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicingPlanDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DicingPlanDetails_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicingPlanDetails_DicingPlans_DicingPlanID",
                        column: x => x.DicingPlanID,
                        principalTable: "DicingPlans",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicingPlanDetails_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PackingPlanMixArticles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<int>(nullable: false),
                    MixPercent = table.Column<decimal>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PackingPlanMixID = table.Column<int>(nullable: false),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    TotalWeight = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingPlanMixArticles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PackingPlanMixArticles_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackingPlanMixArticles_PackingPlanMixs_PackingPlanMixID",
                        column: x => x.PackingPlanMixID,
                        principalTable: "PackingPlanMixs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackingPlanMixArticles_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCategories_Name",
                table: "ArticleCategories",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleCategoryID",
                table: "Articles",
                column: "ArticleCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticlePackageFormID",
                table: "Articles",
                column: "ArticlePackageFormID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleUnitID",
                table: "Articles",
                column: "ArticleUnitID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CountryID",
                table: "Articles",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Name_ArticleUnitID",
                table: "Articles",
                columns: new[] { "Name", "ArticleUnitID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalances_CompanyID",
                table: "ArticleWarehouseBalances",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalances_WarehouseID",
                table: "ArticleWarehouseBalances",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalances_ArticleID_WarehouseID_CompanyID",
                table: "ArticleWarehouseBalances",
                columns: new[] { "ArticleID", "WarehouseID", "CompanyID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name",
                table: "Companies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanySections_CompanyID",
                table: "CompanySections",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySections_Name_CompanyID",
                table: "CompanySections",
                columns: new[] { "Name", "CompanyID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlanDetails_ArticleID",
                table: "DicingPlanDetails",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlanDetails_WarehouseID",
                table: "DicingPlanDetails",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlanDetails_DicingPlanID_WarehouseID_ArticleID",
                table: "DicingPlanDetails",
                columns: new[] { "DicingPlanID", "WarehouseID", "ArticleID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlans_ArticleID",
                table: "DicingPlans",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlans_CompanyID",
                table: "DicingPlans",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlans_ManagerID",
                table: "DicingPlans",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlans_WarehouseID",
                table: "DicingPlans",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ArticleID",
                table: "OrderDetails",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_WarehouseID",
                table: "OrderDetails",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID_ArticleID_WarehouseID_Price",
                table: "OrderDetails",
                columns: new[] { "OrderID", "ArticleID", "WarehouseID", "Price" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CompanyID",
                table: "Orders",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeID",
                table: "Orders",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders",
                column: "OrderTransportID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderTranports_Name",
                table: "OrderTranports",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackagingMaterials_ArticleUnitID",
                table: "PackagingMaterials",
                column: "ArticleUnitID");

            migrationBuilder.CreateIndex(
                name: "IX_PackagingMaterials_PackagingCategoryID",
                table: "PackagingMaterials",
                column: "PackagingCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PackagingMaterials_Name_ArticleUnitID",
                table: "PackagingMaterials",
                columns: new[] { "Name", "ArticleUnitID" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixArticles_ArticleID",
                table: "PackingPlanMixArticles",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixArticles_PackingPlanMixID",
                table: "PackingPlanMixArticles",
                column: "PackingPlanMixID");

            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixArticles_WarehouseID",
                table: "PackingPlanMixArticles",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixs_NewArticleID",
                table: "PackingPlanMixs",
                column: "NewArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixs_PackingPlanID",
                table: "PackingPlanMixs",
                column: "PackingPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixs_ToWarehouseID",
                table: "PackingPlanMixs",
                column: "ToWarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticles_ArticleID",
                table: "PurchasedArticles",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticles_CompanyID",
                table: "PurchasedArticles",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleWarehouses_WarehouseID",
                table: "PurchasedArticleWarehouses",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleWarehouses_PurchasedArticleID_WarehouseID",
                table: "PurchasedArticleWarehouses",
                columns: new[] { "PurchasedArticleID", "WarehouseID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_CompanyID",
                table: "RoastingPlans",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_ManagerID",
                table: "RoastingPlans",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_ArticleID_Date",
                table: "RoastingPlans",
                columns: new[] { "ArticleID", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CompanySectionID",
                table: "Warehouses",
                column: "CompanySectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Name",
                table: "Warehouses",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleWarehouseBalances");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DicingPlanDetails");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "PackagingMaterials");

            migrationBuilder.DropTable(
                name: "PackingPlanMixArticles");

            migrationBuilder.DropTable(
                name: "PurchasedArticleWarehouses");

            migrationBuilder.DropTable(
                name: "RoastingPlans");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DicingPlans");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PackagingCategory");

            migrationBuilder.DropTable(
                name: "PackingPlanMixs");

            migrationBuilder.DropTable(
                name: "PurchasedArticles");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "OrderTranports");

            migrationBuilder.DropTable(
                name: "PackingPlans");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "CompanySections");

            migrationBuilder.DropTable(
                name: "ArticleCategories");

            migrationBuilder.DropTable(
                name: "ArticlePakageForms");

            migrationBuilder.DropTable(
                name: "ArticleUnits");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
