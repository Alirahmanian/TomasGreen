using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Import.ViewModels
{
    public class OnthewayArticlesViewModel
    {
        public PurchasedArticleWarehouse PurchasedArticleWarehouse { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PurchasedDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpectedToArrive { get; set; }
        public Article Article { get; set; }
        public Warehouse Warehouse { get; set; }
        public Company Company { get; set; }
        public string ContainerNumber { get; set; }

    }
}
