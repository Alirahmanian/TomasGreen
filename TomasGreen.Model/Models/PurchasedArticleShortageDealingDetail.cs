using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
   public class PurchasedArticleShortageDealingDetail : BaseEntity
    {
        public int PurchasedArticleID { get; set; }
        public int CompanyID { get; set; }
        public int CurrencyID { get; set; }
        public decimal Amount { get; set; }
        public decimal Description { get; set; }
        public Guid? Guid { get; set; }

        public PurchasedArticle PurchasedArticle { get; set; }
        public Company Company { get; set; }
        public Currency Currency { get; set; }
    }
}
