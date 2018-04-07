using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TomasGreen.Model.Models
{
    public class Article :BaseEntity
    {
        [Required]
        [Display(Name = "Article category")]
        public int ArticleCategoryID { get; set; }
        [Required]
        [Display(Name = "Unit")]
        public int ArticleUnitID { get; set; }
        
        [Required]
        [Display(Name = "Package form")]
        public int ArticlePackageFormID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        [Display(Name = "Min. price/USD")]
        public decimal MinimumPricePerUSD { get; set; } = 0;
        [Required]
        [Range(0, Int32.MaxValue)]
        public decimal WeightPerPackage { get; set; } = 0;

        [Display(Name = "Country")]
        public int CountryID { get; set; }



        //nav.
        public ArticleCategory ArticleCategory { get; set; }
        public ArticleUnit ArticleUnit { get; set; }
        public ArticlePackageForm ArticlePackageForm { get; set; }
        public Country Country { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        //public ICollection<RoastingPlan> RoastingPlans { get; set; }
        // public ArticleWarehouseBalance ArticleWarehouseBalance { get; set; }

    }
}