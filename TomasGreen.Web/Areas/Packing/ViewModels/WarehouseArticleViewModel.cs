using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Packing.ViewModels
{
    public class WarehouseArticleViewModel
    {
        public Warehouse Warehouse { get; set; }
        public Article Article { get; set; }
        public int QtyPackages { get; set; }
        public decimal QtyExtra { get; set; }
    }
}
