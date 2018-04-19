using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
   public class PurchaseArticleShortageDealingDetail : BaseEntity
    {
        public int PurchaseArticleID { get; set; }
        public int CompanyID { get; set; }
        public int CurrencyID { get; set; }
        public bool Cash { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public PurchaseArticle PurchaseArticle { get; set; }
        public Company Company { get; set; }
        public Currency Currency { get; set; }
    }
}
