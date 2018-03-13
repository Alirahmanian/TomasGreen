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
        public int ManagerID { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please choose a company.")]
        public int CompanyID { get; set; }


        //In
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public int FromWarehouseID { get; set; }
        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public int ArticleID { get; set; }
        public int QtyPackages { get; set; }
        public decimal QtyExtra { get; set; }
        public decimal TotalWeight { get; set; }

        //work&material
        public decimal WeightChange { get; set; }
        public decimal Salt { get; set; }
        public decimal SaltLimit { get; set; }// 2% of total weight
        public int PackagingMaterialPackageID { get; set; }
        public int Packages{ get; set; }
        public int PackagingMaterialBagID { get; set; }
        public int Bags { get; set; }

        //New article
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public int ToWarehouseID { get; set; }
        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public int NewArticleID { get; set; }
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
