using System;
using System.ComponentModel.DataAnnotations;

namespace TomasGreen.Model.Models
{
    public class ReceiveArticle : BaseEntity
    {
        public DateTime Date { get; set; }
        [Required(ErrorMessage ="Please choose an article.")]
        [Display(Name = "Article")]
        public Int64 ArticleID { get; set; }
        [Display(Name = "Company")]
        public Int64? CompanyID { get; set; }
        public string Description { get; set; }
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose an warehouse.")]
        public int WarehouseID { get; set; }
        [Display(Name = "Boxes")]
        [Required(ErrorMessage ="Please put boxes.")]
        [Range(0, Int32.MaxValue)]
        public int QtyBoxes { get; set; }
        [Display(Name = "Extra kg")]
        [Range(0, Int32.MaxValue)]
        public decimal QtyKg { get; set; }

        public virtual Article Article { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Company Company { get; set; }
    }
}