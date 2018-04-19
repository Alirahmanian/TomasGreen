using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Import.ViewModels
{
    public class ArrivedArticleViewModel
    {
        public int PurchaseArticleWarehouseID { get; set; }
        public string ContainerNumber { get; set; }
        public Article Article { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ArrivedDate { get; set; }
        [Display(Name = "Packages arrived")]
        [Required(ErrorMessage = "Please put packages.")]
        [Range(0, Int32.MaxValue)]
        public int QtyPackagesArrived { get; set; } = 0;
        [Display(Name = "Extra arrived")]
        [Range(0, Int32.MaxValue)]
        public decimal QtyExtraArrived { get; set; } = 0;

        public Warehouse Warehouse { get; set; }
        public string Notes { get; set; }

    }
}
