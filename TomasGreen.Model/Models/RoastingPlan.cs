using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class RoastingPlan : BaseEntity
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Display(Name = "Manager")]
        [Required(ErrorMessage = "Please choose a manager.")]
        public Int64 ManagerID { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please choose a company.")]
        public Int64 CompanyID { get; set; }


        //In
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public Int64 FromWarehouseID { get; set; }
        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public Int64 ArticleID { get; set; }
        public int QtyPackages { get; set; }
        public decimal QtyExtra { get; set; }
        public decimal TotalWeight { get; set; }

        //work&material
        public decimal WeightChange { get; set; }
        public decimal Salt { get; set; }
        public decimal SaltLimit { get; set; }// 2% of total weight
        public Int64 PackagingMaterialPackageID { get; set; }
        public int Packages{ get; set; }
        public Int64 PackagingMaterialBagID { get; set; }
        public int Bags { get; set; }

        //New article
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public Int64 ToWarehouseID { get; set; }
        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public Int64 NewArticleID { get; set; }
        public int NewQtyPackages { get; set; }
        public decimal NewQtyExtra { get; set; }
        public decimal NewTotalWeight { get; set; }

        //price
        public decimal PricePerUnit { get; set; }
        public decimal TotalPrice { get; set; } //PricePerUnit * NewTotalWeight



        //nav
        
     
        public Company Company { get; set; }
        public Employee Manager { get; set; }
        


    }
}
