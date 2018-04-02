using System;
using System.ComponentModel.DataAnnotations;
namespace TomasGreen.Model.Models
{
    public class OrderDetail : BaseEntity
    {
        [Display(Name ="Order")]
        [Required(ErrorMessage = "Please choose an order.")]
        public int OrderID { get; set; }
        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public int ArticleID { get; set; }
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public int WarehouseID { get; set; }
        //[DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please put price")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; } = 0;
        //[DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal Extended_Price { get; set; } = 0;
        [Range(0, Int32.MaxValue)]
        public int QtyPackages { get; set; } = 0;
        //[Range(0, Int32.MaxValue)]
       // public int QtyReserveBoxes { get; set; } = 0;
        [Range(0, double.MaxValue)]
        public decimal QtyExtra { get; set; } = 0;

        public string Notes { get; set; }

        //nav.
        public Order Order { get; set; }
        public Article Article { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}