using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
   public class PurchaseArticleContainerDetail : BaseEntity
    {
        public int PurchaseArticleID { get; set; }
        [Display(Name = "Container")]
        public string ContainerNumber { get; set; }

        public PurchaseArticle PurchaseArticle { get; set; }
    }
}
