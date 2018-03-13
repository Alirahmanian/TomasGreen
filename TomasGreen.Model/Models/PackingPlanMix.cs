using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class PackingPlanMix : BaseEntity
    {
        public int PackingPlanID { get; set; }
        public int PackagingMaterialPackageID { get; set; }
        public int Packages { get; set; }
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

        public Guid? Guid { get; set; }

        //price
        public decimal PricePerUnit { get; set; }
        public decimal TotalPrice { get; set; }
        //nav
        public ICollection<PackingPlanMixArticle> MixArticles { get; set; }
        public Warehouse ToWarehouse { get; set; }
        public Article NewArticle { get; set; }

    }
}
