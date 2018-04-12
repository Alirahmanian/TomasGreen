using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace TomasGreen.Model.Models
{
    public class PurchasedArticle : BaseEntity
    {
        public PurchasedArticle()
        {
            PurchasedArticleDetails = new HashSet<PurchasedArticleDetail>();
            PurchasedArticleCostDetails = new HashSet<PurchasedArticleCostDetail>();
            PurchasedArticleContainerDetails = new HashSet<PurchasedArticleContainerDetail>();
        }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage ="Please choose an article.")]
        [Display(Name = "Company")]
        public int CompanyID { get; set; }
        [Display(Name = "Currency")]
        public int CurrencyID { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
       
        [Display(Name = "Total price")]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; } = 0;
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpectedToArrive { get; set; }
        public bool HasIssue { get; set; }
        public bool Arrived { get; set; }

        //nav.
        public virtual Company Company { get; set; }
        public virtual IEnumerable<PurchasedArticleDetail> PurchasedArticleDetails { get; set; }
        public virtual IEnumerable<PurchasedArticleCostDetail> PurchasedArticleCostDetails { get; set; }
        public virtual IEnumerable<PurchasedArticleShortageDealingDetail> PurchasedArticleShortageDealingDetails { get; set; }
        public virtual IEnumerable<PurchasedArticleContainerDetail> PurchasedArticleContainerDetails { get; set; }

        public decimal GetTotalPrice()
        {
            decimal result = 0;
            foreach (var purchasedArticleDetail in PurchasedArticleDetails)
            {
                result += purchasedArticleDetail.TotalPerUnit;
            }
            return result;
        }
    }
}