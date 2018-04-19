using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TomasGreen.Model.Models
{
    public class PurchaseArticleDetail : BaseEntity
    {
        [Required(ErrorMessage = "Please choose an article.")]
        public int PurchaseArticleID { get; set; }
        [Required(ErrorMessage = "Please choose an Article.")]
        public int ArticleID { get; set; }
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public int WarehouseID { get; set; }
        [Display(Name = "Packages")]
        [Required(ErrorMessage = "Please put packages.")]
        [Range(0, Int32.MaxValue)]
        public int QtyPackages { get; set; } = 0;
        [Display(Name = "Extra")]
        [Range(0, Int32.MaxValue)]
        public decimal QtyExtra { get; set; } = 0;
        [Display(Name = "Unit price")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal UnitPrice { get; set; } = 0;
        [Display(Name = "Total per unit")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal TotalPerUnit { get; set; } = 0;

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
        public virtual Article Article { get; set; }
        public virtual PurchaseArticle PurchaseArticle { get; set; }
        public virtual Warehouse OntheWayWarehouse { get; set; }
        public virtual Warehouse ArrivedAtWarehouse { get; set; }

        //
        public bool HasIssue()
        {
            if (QtyPackages > QtyPackagesArrived || QtyExtra > QtyExtraArrived)
                return true;
            return false;
        }
    }
}
