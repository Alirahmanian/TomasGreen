using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Sales.ViewModels
{
    public class SaveOrderViewModel
    {
        public Order Order { get; set; }
        public ArticleCategory ArticleCategory { get; set; }
        public OrderDetail OrderDetail { get; set; }
    }
}
