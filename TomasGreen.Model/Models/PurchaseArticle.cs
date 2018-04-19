using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace TomasGreen.Model.Models
{
    public class PurchaseArticle : BaseEntity
    {
        public PurchaseArticle()
        {
            PurchaseArticleDetails = new HashSet<PurchaseArticleDetail>();
            PurchaseArticleCostDetails = new HashSet<PurchaseArticleCostDetail>();
            PurchaseArticleContainerDetails = new HashSet<PurchaseArticleContainerDetail>();
        }

        //relations
        [Display(Name = "Company")]
        public int CompanyID { get; set; }
        [Display(Name = "Currency")]
        public int CurrencyID { get; set; }
        [Display(Name = "Section")]
        public int PurchaserCompanySectionID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage ="Please choose an article.")]
        
        public bool Cash { get; set; }
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
        public CompanySection PurchaserCompanySection { get; set; }
        public virtual IEnumerable<PurchaseArticleDetail> PurchaseArticleDetails { get; set; }
        public virtual IEnumerable<PurchaseArticleCostDetail> PurchaseArticleCostDetails { get; set; }
        public virtual IEnumerable<PurchaseArticleShortageDealingDetail> PurchaseArticleShortageDealingDetails { get; set; }
        public virtual IEnumerable<PurchaseArticleContainerDetail> PurchaseArticleContainerDetails { get; set; }

        public decimal GetTotalPrice()
        {
            decimal result = 0;
            foreach (var PurchaseArticleDetail in PurchaseArticleDetails)
            {
                result += PurchaseArticleDetail.TotalPerUnit;
            }
            return result;
        }
    }
}