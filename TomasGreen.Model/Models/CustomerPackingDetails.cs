using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CustomerPackingDetails :BaseEntity
    {
        public Int64 CustomerPackingID { get; set; }
        public Int64 ArticleID { get; set; }
        public Int64 ManagerID { get; set; }
        public int QtyPackages { get; set; }
        public decimal QtExtra { get; set; }
        public int ArticleUnitID { get; set; }
        public decimal WeightPerPackage { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public bool MIx { get; set; }
    }
}
