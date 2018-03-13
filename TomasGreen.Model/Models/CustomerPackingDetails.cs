using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CustomerPackingDetails : BaseEntity
    {
        public int CustomerPackingID { get; set; }
        public int WarehouseID { get; set; }
        public int ArticleID { get; set; }
        public int QtyPackages { get; set; }
        public decimal QtExtra { get; set; }
        public int ArticleUnitID { get; set; }
        public decimal WeightPerPackage { get; set; }
        public decimal TotalWeight { get; set; }
        public int packagingMaterialBoxID { get; set; }
        public int AmountpackagingMaterialBox { get; set; }
        public int packagingMaterialBagID { get; set; }
        public decimal TotalPrice { get; set; }
        //New Articl + osv
        public bool MIx { get; set; } //false

    }
}
