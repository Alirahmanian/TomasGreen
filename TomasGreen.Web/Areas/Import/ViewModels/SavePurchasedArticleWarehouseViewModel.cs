using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Import.ViewModels
{
    public class SavePurchasedArticleWarehouseViewModel
    {
        public Int64 ID { get; set; }
        //public List<Article> Articles { get; set; }
        //public List<Company> Companies { get; set; }
        public PurchasedArticle PurchasedArticle { get; set; }
        public List<PurchasedArticleWarehouse> PurchasedArticleWarehouses { get; set; }
    }
}
