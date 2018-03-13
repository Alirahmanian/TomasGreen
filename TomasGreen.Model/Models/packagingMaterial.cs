using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class PackagingMaterial : BaseEntity
    {
        public string Name { get; set; }
        public decimal Dimensions { get; set; }
        public int ArticleUnitID { get; set; }
        public decimal Volume { get; set; }
        public string Description { get; set; }
        public int PackagingCategoryID { get; set; }
        public decimal Price { get; set; }

        //nav
        public ArticleUnit ArticleUnit { get; set; }
        public PackagingCategory PackagingCategory { get; set; }
    }
}
