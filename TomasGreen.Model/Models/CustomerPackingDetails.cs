using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CustomerPackingDetails : BaseEntity
    {
        public Int64 CustomerPackingID { get; set; }
        public Int64 WarehouseID { get; set; }
        public Int64 ArticleID { get; set; }
        public int QtyPackages { get; set; }
        public decimal QtExtra { get; set; }
        public int ArticleUnitID { get; set; }
        public decimal WeightPerPackage { get; set; }
        public decimal TotalWeight { get; set; }
        public Int64 packagingMaterialBoxID { get; set; }
        public int AmountpackagingMaterialBox { get; set; }
        public Int64 packagingMaterialBagID { get; set; }
        public decimal TotalPrice { get; set; }
        //New Articl + osv
        public bool MIx { get; set; } //false

    }
}
