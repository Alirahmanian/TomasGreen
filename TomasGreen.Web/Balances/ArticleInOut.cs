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
        public int QtyPackages { get; set; }
        public decimal QtyExtra { get; set; }

    }
}
