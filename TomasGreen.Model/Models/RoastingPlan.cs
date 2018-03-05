using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class RoastingPlan : BaseEntity
    {
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public Int64 WarehouseID { get; set; }
        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public Int64 ArticleID { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please choose a company.")]
        public Int64 CompanyID { get; set; }
        public Int64 ManagerID { get; set; }
        public DateTime Date { get; set; }
        public int QtyPackages { get; set; }
        public decimal QtExtra { get; set; }
        public int ArticleUnitID { get; set; }
        public decimal WeightPerPackage { get; set; }
        public decimal TotalWeight { get; set; }

        public decimal WeightChange { get; set; }
        //New article
        public Int64  NewArticleID { get; set; }
        public int NewQtyPackages { get; set; }
        public decimal NewQtyExtra { get; set; }
        public int NewArticleUnitID { get; set; }
        public decimal NewWeightPerPackage { get; set; }
        public decimal NewTotalWeight { get; set; }
        public decimal Salt { get; set; }
        public decimal SaltLimit { get; set; }// 2% of total weight
        public Int64 NewPackagingMaterialPackageID { get; set; }
        public int NewPackages { get; set; }
        public Int64 NewPackagingMaterialBagID { get; set; }
        public int NewBags { get; set; }

        //
        public decimal PricePerUnit { get; set; }
        public decimal TotalPrice { get; set; } //PricePerUnit * NewTotalWeight



        //nav
        public Warehouse Warehouse { get; set; }
        public Article Article { get; set; }
        public Company Company { get; set; }
        public Employee Manager { get; set; }
        public PackagingMaterial NewPackage { get; set; }
        public PackagingMaterial Newbag { get; set; }

    }
}
