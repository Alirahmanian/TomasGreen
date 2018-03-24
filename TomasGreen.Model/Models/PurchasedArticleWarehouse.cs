using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace TomasGreen.Model.Models
{
    public class PurchasedArticleWarehouse : BaseEntity
    {
        [Required(ErrorMessage = "Please choose an article.")]
        public int PurchasedArticleID { get; set; }
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public int WarehouseID { get; set; }
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

        public int? ArrivedAtWarehouseID { get; set; }
        public string Notes { get; set; }

        //nav
        public virtual PurchasedArticle PurchasedArticle { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        //
        public bool HasIssue()
        {
            if (QtyPackages > QtyPackagesArrived || QtyExtra > QtyExtraArrived)
                return true;
            return false;
        }
    }
}
