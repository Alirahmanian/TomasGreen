using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class DicingPlanDetail :BaseEntity
    {
        public int DicingPlanID { get; set; }
        //Some dicing details
        //Out
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public int WarehouseID { get; set; }
        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public int ArticleID { get; set; }
        public int QtyPackages { get; set; }
        public decimal QtyExtra { get; set; }
        public decimal TotalWeight { get; set; }

        //nav
        public DicingPlan DicingPlan { get; set; }
        public Warehouse Warehouse { get; set; }
        public Article Article { get; set; }

    }
}
