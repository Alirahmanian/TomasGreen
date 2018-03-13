using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace TomasGreen.Model.Models
{
    public class PurchasedArticle : BaseEntity
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage ="Please choose an article.")]
        [Display(Name = "Article")]
        public int ArticleID { get; set; }
        [Display(Name = "Company")]
        public int? CompanyID { get; set; }
        [Display(Name = "Container")]
        public string ContainerNumber { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public Guid? Guid { get; set; }
        public bool Archive { get; set; }
        public bool Received { get; set; }
        [Display(Name = "Toll fee")]
        public decimal TollFee { get; set; } = 0;
        [Display(Name = "Penalty fee")]
        public decimal PenaltyFee { get; set; } = 0;
        public decimal Discount { get; set; }
        [Display(Name = "Transport fee")]
        public decimal TransportCost { get; set; } = 0;
        [Display(Name = "Unit price")]
        public decimal UnitPrice { get; set; } = 0;
        [Display(Name = "Total price")]
        public decimal TotalPrice { get; set; } = 0;
        [Display(Name = "Total per unit")]
        public decimal TotalPerUnit { get; set; } = 0;
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpectedToArrive { get; set; }

        public virtual Article Article { get; set; }
        public virtual Company Company { get; set; }
        public virtual IEnumerable<PurchasedArticleWarehouse> Warehouses { get; set; }
    }
}