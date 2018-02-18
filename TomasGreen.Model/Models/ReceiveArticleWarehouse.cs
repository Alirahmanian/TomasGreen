using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace TomasGreen.Model.Models
{
    public class ReceiveArticleWarehouse : BaseEntity
    {
        [Required(ErrorMessage = "Please choose an received article.")]
        public Int64 ReceiveArticleID { get; set; }
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public Int64 WarehouseID { get; set; }
        [Display(Name = "Boxes")]
        [Required(ErrorMessage = "Please put boxes.")]
        [Range(0, Int32.MaxValue)]
        public int QtyBoxes { get; set; } = 0;
        [Display(Name = "Extra kg")]
        [Range(0, Int32.MaxValue)]
        public decimal QtyExtraKg { get; set; } = 0;

        //nav
        public virtual ReceiveArticle ReceiveArticle { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
