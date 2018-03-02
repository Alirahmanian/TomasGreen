using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace TomasGreen.Model.Models
{
    public class PurchasedArticleWarehouse : BaseEntity
    {
        [Required(ErrorMessage = "Please choose an article.")]
        public Int64 PurchasedArticleID { get; set; }
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public Int64 WarehouseID { get; set; }
        [Display(Name = "Packages")]
        [Required(ErrorMessage = "Please put packages.")]
        [Range(0, Int32.MaxValue)]
        public int QtyPackages { get; set; } = 0;
        [Display(Name = "Extra")]
        [Range(0, Int32.MaxValue)]
        public decimal QtyExtra { get; set; } = 0;

        //Arrived
        public DateTime? ArrivedDate{ get; set; }
        [Display(Name = "Packages arrived")]
        [Required(ErrorMessage = "Please put packages.")]
        [Range(0, Int32.MaxValue)]
        public int QtyPackagesArrived { get; set; } = 0;
        [Display(Name = "Extra arrived")]
        [Range(0, Int32.MaxValue)]
        public decimal QtyExtraArrived { get; set; } = 0;


        //nav
        public virtual PurchasedArticle PurchasedArticle { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
