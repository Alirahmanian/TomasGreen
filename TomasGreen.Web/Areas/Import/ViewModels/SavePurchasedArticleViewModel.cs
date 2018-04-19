using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Import.ViewModels
{
    public class SavePurchaseArticleViewModel
    {
        public PurchaseArticle PurchaseArticle { get; set; }
        public PurchaseArticleDetail PurchaseArticleDetail { get; set; }
        public PurchaseArticleCostDetail PurchaseArticleCostDetail { get; set; }
        public PurchaseArticleShortageDealingDetail PurchaseArticleShortageDealingDetail { get; set; }
        public PurchaseArticleContainerDetail PurchaseArticleContainerDetail { get; set; }
        public int ActiveTab { get; set; }
    }
}
