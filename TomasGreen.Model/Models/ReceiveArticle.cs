using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace TomasGreen.Model.Models
{
    public class ReceiveArticle : BaseEntity
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage ="Please choose an article.")]
        [Display(Name = "Article")]
        public Int64 ArticleID { get; set; }
        [Display(Name = "Company")]
        public Int64? CompanyID { get; set; }
        [Display(Name = "Container")]
        public string ContainerNumber { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public Guid? Guid { get; set; }



        public virtual Article Article { get; set; }
        public virtual Company Company { get; set; }
        public virtual IEnumerable<ReceiveArticleWarehouse> Warehouses { get; set; }
    }
}