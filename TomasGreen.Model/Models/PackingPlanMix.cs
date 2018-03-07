using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class PackingPlanMix : BaseEntity
    {
        public Int64 PackingPlanID { get; set; }
        public Int64 PackagingMaterialPackageID { get; set; }
        public int Packages { get; set; }
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

        //nav
        public ICollection<PackingPlanMixArticle> MixArticles { get; set; }

    }
}
