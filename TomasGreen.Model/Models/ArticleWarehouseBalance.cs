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
        public decimal QtyKgIn { get; set; }
        public decimal QtyTotalWeightIn { get; set; }
        public int QtyBoxesOnhand { get; set; }
        public decimal QtyKgOnhand { get; set; }
        public decimal QtyTotalWeightOnhand { get; set; }
        public int QtyBoxesReserved { get; set; }
        public decimal QtyTotalWeightReserved { get; set; }
        public virtual Article Article { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
