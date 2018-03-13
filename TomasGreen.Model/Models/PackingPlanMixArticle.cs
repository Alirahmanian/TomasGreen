using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class PackingPlanMixArticle : BaseEntity
    {
        public int PackingPlanMixID { get; set; }
        //In
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public int WarehouseID { get; set; }
        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public int ArticleID { get; set; }
        public int QtyPackages { get; set; }
        public decimal QtyExtra { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal MixPercent { get; set; }

        //nav
        public PackingPlanMix PackingPlanMix { get; set; }
        public Warehouse Warehouse { get; set; }
        public Article Article { get; set; }
    }
}
