using System;
using System.ComponentModel.DataAnnotations;
namespace TomasGreen.Model.Models
{
    public class OrderDetail : BaseEntity
    {
        [Display(Name ="Order")]
        [Required(ErrorMessage = "Please choose an order.")]
        public Int64 OrderID { get; set; }
        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public Int64 ArticleID { get; set; }
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public Int64 WarehouseID { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage ="Please put price")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal Extended_Price { get; set; }
        [Range(0, Int32.MaxValue)]
        public int QtyBoxes { get; set; }
        [Range(0, Int32.MaxValue)]
        public int QtyReservBoxes { get; set; }
        [Range(0, Int32.MaxValue)]
        public decimal QtyKg { get; set; }

        //nav.
        public Order Order { get; set; }
        public Article Article { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}