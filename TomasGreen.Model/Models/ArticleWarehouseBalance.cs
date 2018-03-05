﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class ArticleWarehouseBalance : BaseEntity
    {
        public Int64 ArticleID { get; set; }
        public Int64 WarehouseID { get; set; }
        public Int64 CompanyID { get; set; }
        public int QtyPackagesIn { get; set; }
        public decimal QtyExtraIn { get; set; }

        public int QtyPackagesOut { get; set; }
        public decimal QtyExtraOut { get; set; }

        public int QtyPackagesOnhand { get; set; }
        public decimal QtyExtraOnhand { get; set; }



        //nav
        public virtual Article Article { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Company Company { get; set; }

    }
}
