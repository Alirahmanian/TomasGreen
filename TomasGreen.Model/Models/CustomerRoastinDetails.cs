using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    class CustomerRoastinDetails : BaseEntity
    {
        public Int64 CustomerRoastingID { get; set; }
        public Int64 ArticleID { get; set; }
        public Int64 ManagerID { get; set; }
        public int QtyPackages { get; set; }
        public decimal QtExtra { get; set; }
        public int ArticleUnitID { get; set; }
        public decimal WeightPerPackage { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        //Tech.
        public decimal Humidity { get; set; }
        public decimal Oil { get; set; }
        public decimal Water { get; set; }
        public decimal Salt { get; set; }
        public decimal Spice { get; set; }
        
        public decimal Heat { get; set; }
        public decimal RoastingPricePerKg { get; set; }
        public decimal TotalRoastingPrice { get; set; }
        public decimal ExpectedWeightLossPerKg { get; set; }
        public decimal ActualWeightLossPerKg { get; set; }
        public decimal TotalWeightLoss { get; set; }
        public decimal ExpectedWeightGainPerKg { get; set; }
        public decimal ActualWeightGainPerKg { get; set; }
        public decimal TotalWeightGain { get; set; }

        //Packing
        public bool PackagesReused { get; set; }
        public decimal TotalPackingPrice { get; set; }



    }
}
