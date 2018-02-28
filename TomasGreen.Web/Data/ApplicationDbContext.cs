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
        public DbSet<OrderTransport> OrderTranports { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<PurchasedArticle> PurchasedArticles { get; set; }
        public DbSet<PurchasedArticleWarehouse> PurchasedArticleWarehouses { get; set; }
        public DbSet<ArticleWarehouseBalance> ArticleWarehouseBalances { get; set; }
        public DbSet<ArticleUnit> ArticleUnits { get; set; }
        public DbSet<ArticlePackageForm> ArticlePakageForms { get; set; }

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
            //modelBuilder.Entity<Employee>()
            //.HasIndex(p => new { p.FirstName, p.LastName })
            //.IsUnique(true);
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
            modelBuilder.Entity<Warehouse>()
           .HasIndex(p => p.Name)
           .IsUnique(true);
            modelBuilder.Entity<OrderTransport>()
            .HasIndex(p => p.Name)
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
                .HasForeignKey(p => p.PurchasedArticleID);

            modelBuilder.Entity<PurchasedArticleWarehouse>()
                .HasOne(p => p.Warehouse)
                .WithMany(p => p.PurchasedArticles)
                .HasForeignKey(p => p.WarehouseID);

            modelBuilder.Entity<ArticleWarehouseBalance>()
               .HasIndex(p => new { p.ArticleID, p.WarehouseID })
               .IsUnique(true);

            // modelBuilder.Entity<UserType>()
            // .HasIndex(p => p.Name)
            // .IsUnique(true);
            // modelBuilder.Entity<UserType>()
            //.HasIndex(p => p.DbTableName)
            //.IsUnique(true);



            base.OnModelCreating(modelBuilder);

        }

        
    }
}
