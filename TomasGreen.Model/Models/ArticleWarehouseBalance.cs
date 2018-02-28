using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class ArticleWarehouseBalance : BaseEntity
    {
        public Int64 ArticleID { get; set; }
        public Int64 WarehouseID { get; set; }
        public int QtyPackagesIn { get; set; }
        public decimal QtyExtraIn { get; set; }
        public decimal QtyTotalIn { get; set; }

        public int QtyPackagesOut { get; set; }
        public decimal QtyExtraOut { get; set; }
        public decimal QtyTotalOut { get; set; }

        public int QtyPackagesOnhand { get; set; }
        public decimal QtyExtraOnhand { get; set; }
        public decimal QtyTotalOnhand { get; set; }
        //public decimal QtyTotalOnhand { get { return ((QtyPackagesOnhand * this.Article.WeightPerPackage) + QtyExtraOnhand); } }



        //nav
        public virtual Article Article { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        
    }
}
