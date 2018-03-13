using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TomasGreen.Web.Balances
{
    public class ArticleInOut
    {
        public int ArticleID { get; set; }
        public int WarehouseID { get; set; }
        public int CompanyID { get; set; }
        public int QtyPackagesIn { get; set; }
        public decimal QtyExtraIn { get; set; }

        public int QtyPackagesOut { get; set; }
        public decimal QtyExtraOut { get; set; }

    }
}
