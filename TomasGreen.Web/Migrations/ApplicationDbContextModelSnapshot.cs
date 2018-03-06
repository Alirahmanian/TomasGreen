﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.Article", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<bool>("Archive");

                    b.Property<long>("ArticleCategoryID");

                    b.Property<long>("ArticlePackageFormID");

                    b.Property<long>("ArticleUnitID");

                    b.Property<long>("CountryID");

                    b.Property<string>("Description");

                    b.Property<decimal>("MinimumPricePerUSD");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("UserName");

                    b.Property<decimal>("WeightPerPackage");

                    b.HasKey("ID");

                    b.HasIndex("ArticleCategoryID");

                    b.HasIndex("ArticlePackageFormID");

                    b.HasIndex("ArticleUnitID");

                    b.HasIndex("CountryID");

                    b.HasIndex("Name", "ArticleUnitID")
                        .IsUnique();

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.ArticleCategory", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<bool>("Archive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("ArticleCategories");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.ArticlePackageForm", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.ToTable("ArticlePakageForms");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.ArticleUnit", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<bool>("MeasuresByKg");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.ToTable("ArticleUnits");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.ArticleWarehouseBalance", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<long>("ArticleID");

                    b.Property<long>("CompanyID");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<decimal>("QtyExtraIn");

                    b.Property<decimal>("QtyExtraOnhand");

                    b.Property<decimal>("QtyExtraOut");

                    b.Property<int>("QtyPackagesIn");

                    b.Property<int>("QtyPackagesOnhand");

                    b.Property<int>("QtyPackagesOut");

                    b.Property<string>("UserName");

                    b.Property<long>("WarehouseID");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("WarehouseID");

                    b.HasIndex("ArticleID", "WarehouseID", "CompanyID")
                        .IsUnique();

                    b.ToTable("ArticleWarehouseBalances");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.Company", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<bool>("Archive");

                    b.Property<decimal>("Balance");

                    b.Property<decimal>("CreditLimit");

                    b.Property<decimal>("CreditReceived");

                    b.Property<decimal>("Discount");

                    b.Property<bool>("IsOwner");

                    b.Property<decimal>("LastBalance");

                    b.Property<DateTime>("LastBalanceDate");

                    b.Property<bool>("Locked");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<decimal>("Paid");

                    b.Property<decimal>("Purchases");

                    b.Property<decimal>("Received");

                    b.Property<bool>("Ruble");

                    b.Property<decimal>("SouldToUs");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.CompanySection", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<long>("CompanyID");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("Name", "CompanyID")
                        .IsUnique();

                    b.ToTable("CompanySections");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.Country", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<bool>("Archive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.Employee", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<int?>("Age");

                    b.Property<bool>("Archive");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("ImageUrl");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.Order", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<int>("AmountArticle");

                    b.Property<decimal?>("AmountPaid");

                    b.Property<int>("AmountReserve");

                    b.Property<bool>("Archive");

                    b.Property<bool>("Cash");

                    b.Property<string>("Coments");

                    b.Property<long>("CompanyID");

                    b.Property<bool>("Confirmed");

                    b.Property<long>("EmployeeID");

                    b.Property<bool>("ForcedPaid");

                    b.Property<Guid?>("Guid");

                    b.Property<DateTime?>("LoadedDate");

                    b.Property<DateTime?>("LoadingDate");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<DateTime>("OrderDate");

                    b.Property<bool>("OrderPaid");

                    b.Property<long>("OrderTransportID");

                    b.Property<string>("OrderdBy");

                    b.Property<DateTime?>("PaidDate");

                    b.Property<DateTime?>("PaymentDate");

                    b.Property<string>("PaymentWarning");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("OrderTransportID")
                        .IsUnique();

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.OrderDetail", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<long>("ArticleID");

                    b.Property<decimal>("Extended_Price");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<long>("OrderID");

                    b.Property<decimal>("Price");

                    b.Property<decimal>("QtyExtra");

                    b.Property<int>("QtyPackages");

                    b.Property<string>("UserName");

                    b.Property<long>("WarehouseID");

                    b.HasKey("ID");

                    b.HasIndex("ArticleID");

                    b.HasIndex("WarehouseID");

                    b.HasIndex("OrderID", "ArticleID", "WarehouseID", "Price")
                        .IsUnique();

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.OrderTransport", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<bool>("Archive");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("OrderTranports");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.PackagingCategory", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.ToTable("PackagingCategory");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.PackagingMaterial", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<long>("ArticleUnitID");

                    b.Property<string>("Description");

                    b.Property<decimal>("Dimensions");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name");

                    b.Property<long>("PackagingCategoryID");

                    b.Property<decimal>("Price");

                    b.Property<string>("UserName");

                    b.Property<decimal>("Volume");

                    b.HasKey("ID");

                    b.HasIndex("ArticleUnitID");

                    b.HasIndex("PackagingCategoryID");

                    b.HasIndex("Name", "ArticleUnitID")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("PackagingMaterials");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.PurchasedArticle", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<bool>("Archive");

                    b.Property<long>("ArticleID");

                    b.Property<long?>("CompanyID");

                    b.Property<string>("ContainerNumber");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<decimal>("Discount");

                    b.Property<DateTime?>("ExpectedToArrive");

                    b.Property<Guid?>("Guid");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<decimal>("PenaltyFee");

                    b.Property<bool>("Received");

                    b.Property<decimal>("TollFee");

                    b.Property<decimal>("TotalPerUnit");

                    b.Property<decimal>("TotalPrice");

                    b.Property<decimal>("TransportCost");

                    b.Property<decimal>("UnitPrice");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("ArticleID");

                    b.HasIndex("CompanyID");

                    b.ToTable("PurchasedArticles");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.PurchasedArticleWarehouse", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime?>("ArrivedDate");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<long>("PurchasedArticleID");

                    b.Property<decimal>("QtyExtra");

                    b.Property<decimal>("QtyExtraArrived");

                    b.Property<int>("QtyPackages");

                    b.Property<int>("QtyPackagesArrived");

                    b.Property<string>("UserName");

                    b.Property<long>("WarehouseID");

                    b.HasKey("ID");

                    b.HasIndex("WarehouseID");

                    b.HasIndex("PurchasedArticleID", "WarehouseID")
                        .IsUnique();

                    b.ToTable("PurchasedArticleWarehouses");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.RoastingPlan", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<long>("ArticleID");

                    b.Property<int>("Bags");

                    b.Property<long>("CompanyID");

                    b.Property<DateTime>("Date");

                    b.Property<long>("FromWarehouseID");

                    b.Property<long>("ManagerID");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<long>("NewArticleID");

                    b.Property<decimal>("NewQtyExtra");

                    b.Property<int>("NewQtyPackages");

                    b.Property<decimal>("NewTotalWeight");

                    b.Property<int>("Packages");

                    b.Property<long>("PackagingMaterialBagID");

                    b.Property<long>("PackagingMaterialPackageID");

                    b.Property<decimal>("PricePerUnit");

                    b.Property<decimal>("QtyExtra");

                    b.Property<int>("QtyPackages");

                    b.Property<decimal>("Salt");

                    b.Property<decimal>("SaltLimit");

                    b.Property<long>("ToWarehouseID");

                    b.Property<decimal>("TotalPrice");

                    b.Property<decimal>("TotalWeight");

                    b.Property<string>("UserName");

                    b.Property<decimal>("WeightChange");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("ManagerID");

                    b.HasIndex("ArticleID", "Date")
                        .IsUnique();

                    b.ToTable("RoastingPlans");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.Warehouse", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Address");

                    b.Property<bool>("Archive");

                    b.Property<long?>("CompanySectionID");

                    b.Property<string>("Description");

                    b.Property<bool>("IsCustomers");

                    b.Property<bool>("IsOnTheWay");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Phone");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("CompanySectionID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("TomasGreen.Web.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TomasGreen.Web.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TomasGreen.Web.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Web.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TomasGreen.Web.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TomasGreen.Model.Models.Article", b =>
                {
                    b.HasOne("TomasGreen.Model.Models.ArticleCategory", "ArticleCategory")
                        .WithMany("Articles")
                        .HasForeignKey("ArticleCategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.ArticlePackageForm", "ArticlePackageForm")
                        .WithMany()
                        .HasForeignKey("ArticlePackageFormID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.ArticleUnit", "ArticleUnit")
                        .WithMany()
                        .HasForeignKey("ArticleUnitID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TomasGreen.Model.Models.ArticleWarehouseBalance", b =>
                {
                    b.HasOne("TomasGreen.Model.Models.Article", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TomasGreen.Model.Models.CompanySection", b =>
                {
                    b.HasOne("TomasGreen.Model.Models.Company", "Company")
                        .WithMany("Sections")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TomasGreen.Model.Models.Order", b =>
                {
                    b.HasOne("TomasGreen.Model.Models.Company", "Company")
                        .WithMany("Orders")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.Employee", "Employee")
                        .WithMany("Orders")
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.OrderTransport", "OrderTransport")
                        .WithOne("Orders")
                        .HasForeignKey("TomasGreen.Model.Models.Order", "OrderTransportID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TomasGreen.Model.Models.OrderDetail", b =>
                {
                    b.HasOne("TomasGreen.Model.Models.Article", "Article")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ArticleID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.Warehouse", "Warehouse")
                        .WithMany("OrderDetails")
                        .HasForeignKey("WarehouseID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TomasGreen.Model.Models.PackagingMaterial", b =>
                {
                    b.HasOne("TomasGreen.Model.Models.ArticleUnit", "ArticleUnit")
                        .WithMany()
                        .HasForeignKey("ArticleUnitID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.PackagingCategory", "PackagingCategory")
                        .WithMany()
                        .HasForeignKey("PackagingCategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TomasGreen.Model.Models.PurchasedArticle", b =>
                {
                    b.HasOne("TomasGreen.Model.Models.Article", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID");
                });

            modelBuilder.Entity("TomasGreen.Model.Models.PurchasedArticleWarehouse", b =>
                {
                    b.HasOne("TomasGreen.Model.Models.PurchasedArticle", "PurchasedArticle")
                        .WithMany("Warehouses")
                        .HasForeignKey("PurchasedArticleID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.Warehouse", "Warehouse")
                        .WithMany("PurchasedArticles")
                        .HasForeignKey("WarehouseID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TomasGreen.Model.Models.RoastingPlan", b =>
                {
                    b.HasOne("TomasGreen.Model.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TomasGreen.Model.Models.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TomasGreen.Model.Models.Warehouse", b =>
                {
                    b.HasOne("TomasGreen.Model.Models.CompanySection", "Section")
                        .WithMany()
                        .HasForeignKey("CompanySectionID");
                });
#pragma warning restore 612, 618
        }
    }
}
