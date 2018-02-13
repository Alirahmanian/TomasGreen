using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class ArticleWarehouseBalance : BaseEntity
    {
        public Int64 ArticleID { get; set; }
        public Int64 WarehouseID { get; set; }
        public int QtyBoxesIn { get; set; }
        public decimal QtyExtraKgIn { get; set; }
        public decimal QtyTotalWeightIn { get { return ((QtyBoxesIn * this.Article.BoxWeight) + QtyExtraKgIn); } }

        public int QtyBoxesOut { get; set; }
        public decimal QtyExtraKgOut { get; set; }
        public decimal QtyTotalWeightOut { get { return ((QtyBoxesOut * this.Article.BoxWeight) + QtyExtraKgOut); } }

        public int QtyBoxesReserved { get; set; }
        public decimal QtyTotalWeightReserved { get { return (QtyBoxesReserved * this.Article.BoxWeight); } }
        

        public int QtyBoxesOnhand { get { return (QtyBoxesIn - QtyBoxesOut); } }
        public decimal QtyKgOnhand { get { return (QtyExtraKgIn - QtyExtraKgOut); } }
        public decimal QtyTotalWeightOnhand { get { return (QtyTotalWeightIn - QtyTotalWeightOut); } }



        //nav
        public virtual Article Article { get; set; }
        public virtual Warehouse Warehouse { get; set; }


    }
}
