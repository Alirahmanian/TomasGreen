using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    class packagingMaterial : BaseEntity
    {
        public string Name { get; set; }
        public decimal Dimensions { get; set; }
        public Int64 ArticleUnitID { get; set; }
        public decimal Volume { get; set; }
        public string Description { get; set; }
        public bool PackagingCategoryID { get; set; }
        public decimal Price { get; set; }

        //nav
        public ArticleUnit ArticleUnit { get; set; }
        public PackagingCategory PackagingCategory { get; set; }

    }
}
