using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Stock.ViewModels
{
    public class SaveReceiveArticleWarehouseViewModel
    {
        public Int64 ID { get; set; }
        //public List<Article> Articles { get; set; }
        //public List<Company> Companies { get; set; }
        public ReceiveArticle ReceiveArticle { get; set; }
        public List<ReceiveArticleWarehouse> ReceiveArticleWarehouses { get; set; }
    }
}
