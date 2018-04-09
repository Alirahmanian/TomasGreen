using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
   public class PurchasedArticleContainerDetail : BaseEntity
    {
        public int PurchasedArticleID { get; set; }
        [Display(Name = "Container")]
        public string ContainerNumber { get; set; }

        public PurchasedArticle PurchasedArticle { get; set; }
    }
}
