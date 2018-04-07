using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Import.ViewModels
{
    public class SavePurchasedArticleViewModel
    {
        public PurchasedArticle PurchasedArticle { get; set; }
        public PurchasedArticleDetail PurchasedArticleDetail { get; set; }
        public PurchasedArticleCostDetail PurchasedArticleCostDetail { get; set; }
    }
}
