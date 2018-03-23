using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Import.ViewModels
{
    public class WarehouseDetailsViewModel
    {
        public Warehouse Warehouse { get; set; }
        public ICollection<ArticleWarehouseBalance> Articles { get; set; }
    }
}
