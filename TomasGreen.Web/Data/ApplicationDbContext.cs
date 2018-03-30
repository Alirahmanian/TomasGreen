using System;
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
        public DbSet<PurchasedArticle> PurchasedArticles { get; set; }
        public DbSet<PurchasedArticleWarehouse> PurchasedArticleWarehouses { get; set; }
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


        public virtual async Task<int> SaveChangesAsync()
        {
            PutBaseEntityValues();
            return await base.SaveChangesAsync();
        }
        
        public void PutBaseEntityValues()
        {
            foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity.GetType().GetProperty("AddedDate") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("AddedDate").CurrentValue = entry.Property("AddedDate").CurrentValue?? DateTime.Now;
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
                entry.Property("ModifiedDate").CurrentValue = entry.Property("ModifiedDate").CurrentValue??DateTime.Now;
            }

            foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity.GetType().GetProperty("UserName") != null))
            {
               
               // entry.Property("UserName").CurrentValue = 
            }

           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<OrderDetail>()
            .HasIndex(p => new { p.OrderID, p.ArticleID, p.WarehouseID, p.Price})
            .IsUnique(true);

            modelBuilder.Entity<Warehouse>()
            .HasIndex(p => p.Name)
            .IsUnique(true);
            modelBuilder.Entity<PurchasedArticleWarehouse>()
                .HasIndex(p => new { p.PurchasedArticleID, p.WarehouseID })
                .IsUnique(true);
            modelBuilder.Entity<PurchasedArticleWarehouse>()
                .HasOne(p => p.PurchasedArticle)
                .WithMany(p => p.Warehouses)
                .HasForeignKey(p => p.PurchasedArticleID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchasedArticleWarehouse>()
                .HasOne(p => p.Warehouse)
                .WithMany(p => p.PurchasedArticles)
                .HasForeignKey(p => p.WarehouseID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArticleWarehouseBalance>()
               .HasIndex(p => new { p.ArticleID, p.WarehouseID, p.CompanyID })
               .IsUnique(true);


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
           
            base.OnModelCreating(modelBuilder);

        }

       
    }
}
