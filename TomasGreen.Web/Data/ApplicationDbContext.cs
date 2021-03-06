﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Web.Models;
using TomasGreen.Model.Models;
using System.Reflection;
using TomasGreen.Web.Areas.Import.ViewModels;
using EntityFrameworkCore.Triggers;

namespace TomasGreen.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
          
        }
        //public DbSet<UserType> UserTypes { get; set; }
        //public DbSet<SystemUser> SystemUser { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<PurchaseArticle> PurchaseArticles { get; set; }
        public DbSet<PurchaseArticleDetail> PurchaseArticleDetails { get; set; }
        public DbSet<ArticleWarehouseBalance> ArticleWarehouseBalances { get; set; }
        public DbSet<ArticleUnit> ArticleUnits { get; set; }
        public DbSet<ArticlePackageForm> ArticlePakageForms { get; set; }
        public DbSet<RoastingPlan> RoastingPlans { get; set; }
        public DbSet<PackagingMaterial> PackagingMaterials { get; set; }
        public DbSet<CompanySection> CompanySections { get; set; }
        public DbSet<PackingPlan> PackingPlans { get; set; }
        public DbSet<PackingPlanMix> PackingPlanMixs { get; set; }
        public DbSet<PackingPlanMixArticle> PackingPlanMixArticles { get; set; }
        public DbSet<DicingPlan> DicingPlans { get; set; }
        public DbSet<DicingPlanDetail> DicingPlanDetails { get; set; }
        public DbSet<WarehouseToWarehouse> WarehouseToWarehouses { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExternalApi> ExternalApis { get; set; }
        public DbSet<ExternalApiFunction> ExternalApiFunctions { get; set; }
        public DbSet<CompanyCreditDebitBalance> CompanyCreditDebitBalances { get; set; }
        public DbSet<CompanyCreditDebitBalanceDetail> CompanyCreditDebitBalanceDetails { get; set; }
        public DbSet<CompanyCreditDebitBalanceDetailType> CompanyCreditDebitBalanceDetailTypes { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PurchaseArticleCostType> PurchaseArticleCostTypes { get; set; }
        public DbSet<PurchaseArticleCostDetail> PurchaseArticleCostDetails { get; set; }
        public DbSet<PurchaseArticleContainerDetail> PurchaseArticleContainerDetails { get; set; }
        public DbSet<PurchaseArticleShortageDealingDetail> PurchaseArticleShortageDealingDetails { get; set; }
        public DbSet<ArticleWarehouseBalanceDetail> ArticleWarehouseBalanceDetails { get; set; }
        public DbSet<ArticleWarehouseBalanceDetailType> ArticleWarehouseBalanceDetailTypes { get; set; }
        public DbSet<CompanyToCompanyPayment> CompanyToCompanyPayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleWarehouseBalanceDetailType>().HasIndex(p => new { p.Name }).IsUnique(true);
            modelBuilder.Entity<ArticleWarehouseBalanceDetail>()
             .HasIndex(p => new { p.ArticleID, p.WarehouseID, p.CompanyID, p.ArticleWarehouseBalanceDetailTypeID, p.BalanceChangerID }).IsUnique(true);
            modelBuilder.Entity<PurchaseArticleShortageDealingDetail>().HasIndex(p => new { p.PurchaseArticleID, p.CompanyID, p.CurrencyID }).IsUnique(true);
            modelBuilder.Entity<PurchaseArticleContainerDetail>().HasIndex(p => new { p.PurchaseArticleID, p.ContainerNumber }).IsUnique(true);
            modelBuilder.Entity<PurchaseArticleCostType>().HasIndex(p => new { p.Name }).IsUnique(true);
            modelBuilder.Entity<PurchaseArticleCostDetail>().HasIndex(p => new {p.PurchaseArticleID, p.PaymentTypeID, p.CompanyID, p.CurrencyID  }).IsUnique(true);
            modelBuilder.Entity<CompanyCreditDebitBalance>()
            .HasIndex(p => new { p.CompanyID, p.CurrencyID })
            .IsUnique(true);
            modelBuilder.Entity<CompanyCreditDebitBalanceDetail>()
              .HasIndex(p => new { p.CompanyID, p.CurrencyID, p.CompanyCreditDebitBalanceDetailTypeID, p.BalanceChangerID, p.PaymentTypeID })
             .IsUnique(true);
            modelBuilder.Entity<CompanyCreditDebitBalanceDetailType>()
             .HasIndex(p => new { p.Name })
             .IsUnique(true);
            modelBuilder.Entity<PaymentType>()
             .HasIndex(p => new { p.Name })
             .IsUnique(true);
            modelBuilder.Entity<ExternalApi>()
           .HasIndex(p => new { p.Link, p.Key})
           .IsUnique(true);
            modelBuilder.Entity<ExternalApiFunction>()
           .HasIndex(p => new { p.ExternalApiID, p.Name })
           .IsUnique(true);
            modelBuilder.Entity<Currency>()
            .HasIndex(p => p.Code)
            .IsUnique(true);
            modelBuilder.Entity<WarehouseToWarehouse>()
            .HasIndex(p => new { p.Date, p.SenderWarehouseID, p.RecipientWarehouseID, p.ArticleID })
            .IsUnique(true);
            modelBuilder.Entity<DicingPlanDetail>()
             .HasIndex(p => new{p.DicingPlanID, p.WarehouseID, p.ArticleID })
             .IsUnique(true);
            modelBuilder.Entity<ArticlePackageForm>()
             .HasIndex(p => p.Name)
             .IsUnique(true);
            modelBuilder.Entity<ArticleUnit>()
             .HasIndex(p => p.Name)
             .IsUnique(true);
          
            modelBuilder.Entity<ArticleCategory>()
             .HasIndex(p => p.Name)
             .IsUnique(true);
            modelBuilder.Entity<Article>()
             .HasIndex(p => new { p.Name, p.ArticleUnitID })
             .IsUnique(true);
            modelBuilder.Entity<Employee>()
            .HasIndex(p => p.Email)
            .IsUnique(true);
            modelBuilder.Entity<Country>()
            .HasIndex(p => p.Name)
            .IsUnique(true);
            modelBuilder.Entity<Company>()
           .HasIndex(p => p.Name)
           .IsUnique(true);
            modelBuilder.Entity<CompanySection>()
             .HasIndex(p => new { p.Name, p.CompanyID })
             .IsUnique(true);
            modelBuilder.Entity<Warehouse>()
           .HasIndex(p => p.Name)
           .IsUnique(true);
            modelBuilder.Entity<RoastingPlan>()
             .HasIndex(p => new { p.ArticleID, p.Date })
             .IsUnique(true);
            modelBuilder.Entity<PackagingMaterial>()
              .HasIndex(p => new { p.Name, p.ArticleUnitID })
              .IsUnique(true);
            modelBuilder.Entity<Order>()
            .HasIndex(p => new { p.OrderNumber })
            .IsUnique(true);
            modelBuilder.Entity<OrderDetail>()
            .HasIndex(p => new { p.OrderID, p.ArticleID, p.WarehouseID, p.Price})
            .IsUnique(true);

            modelBuilder.Entity<Warehouse>()
            .HasIndex(p => p.Name)
            .IsUnique(true);
            modelBuilder.Entity<PurchaseArticleDetail>()
                .HasIndex(p => new { p.PurchaseArticleID, p.WarehouseID })
                .IsUnique(true);
            modelBuilder.Entity<PurchaseArticleDetail>()
                .HasOne(p => p.PurchaseArticle)
                .WithMany(p => p.PurchaseArticleDetails)
                .HasForeignKey(p => p.PurchaseArticleID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseArticleDetail>()
                .HasOne(p => p.OntheWayWarehouse)
                .WithMany(p => p.PurchaseArticleDetails)
                .HasForeignKey(p => p.WarehouseID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArticleWarehouseBalance>()
               .HasIndex(p => new { p.ArticleID, p.WarehouseID, p.CompanyID })
               .IsUnique(true);

            //modelBuilder.Entity<PurchaseArticle>().Property(x => x.TotalPrice).HasColumnType("decimal(18, 4)");
            //modelBuilder.Entity<Article>().Property(x => x.MinimumPricePerUSD).HasColumnType("decimal(18, 4)");
            //modelBuilder.Entity<CompanyCreditDebitBalance>().Property(x => x.Credit).HasColumnType("decimal(18, 4)");
            //modelBuilder.Entity<CompanyCreditDebitBalance>().Property(x => x.Debit).HasColumnType("decimal(18, 4)");
            //modelBuilder.Entity<CompanyCreditDebitBalanceDetail>().Property(x => x.Credit).HasColumnType("decimal(18, 4)");
            //modelBuilder.Entity<CompanyCreditDebitBalanceDetail>().Property(x => x.Debit).HasColumnType("decimal(18, 4)");
            //modelBuilder.Entity<OrderDetail>().Property(x => x.Price).HasColumnType("decimal(18, 4)");
            //modelBuilder.Entity<OrderDetail>().Property(x => x.Extended_Price).HasColumnType("decimal(18, 4)");
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal)))
            {
               property.Relational().ColumnType = "decimal(18, 4)";
            }

            base.OnModelCreating(modelBuilder);

        }

        public virtual async Task<int> SaveChangesAsync()
        {
            //Triggers<CompanyCreditDebitBalanceDetailType>.Updating += e => e.Entity.Name = (e.Original.UsedBySystem == true)? e.Original.Name: e.Entity.Name;
            //Triggers<CompanyCreditDebitBalanceDetailType>.Deleting += e => e.Cancel = e.Original.UsedBySystem;
            //Triggers<PaymentType>.Updating += e => e.Entity.Name = (e.Original.UsedBySystem == true) ? e.Original.Name : e.Entity.Name;
            //Triggers<PaymentType>.Deleting += e => e.Cancel = e.Original.UsedBySystem;
            PutBaseEntityValues();
            return await base.SaveChangesAsync();
        }

        private void PutBaseEntityValues()
        {
            foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity.GetType().GetProperty("AddedDate") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("AddedDate").CurrentValue = entry.Property("AddedDate").CurrentValue ?? DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("AddedDate").IsModified = false;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(
                e =>
                    e.Entity.GetType().GetProperty("ModifiedDate") != null &&
                    e.State == EntityState.Modified))
            {
                entry.Property("ModifiedDate").CurrentValue = entry.Property("ModifiedDate").CurrentValue ?? DateTime.Now;
            }

            foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity.GetType().GetProperty("UserName") != null))
            {
                // entry.Property("UserName").CurrentValue = 
            }
            // To protect values sused by the system
            foreach (var entry in ChangeTracker.Entries().Where(c => c.Entity is PaymentType || c.Entity is CompanyCreditDebitBalanceDetailType))
            {
                if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    var isUsedBySystem = (bool)entry.Property("UsedBySystem").OriginalValue;
                    if (isUsedBySystem)
                    {
                        entry.State = EntityState.Unchanged;
                    }
                }
            }
        }
    }
}
