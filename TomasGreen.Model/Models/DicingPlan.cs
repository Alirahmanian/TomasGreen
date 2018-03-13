using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
   public class DicingPlan : BaseEntity
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Display(Name = "Manager")]
        [Required(ErrorMessage = "Please choose a manager.")]
        public int ManagerID { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please choose a company.")]
        public int CompanyID { get; set; }
        [Display(Name = "Warehouse")]
        [Required(ErrorMessage = "Please choose a warehouse.")]
        public int WarehouseID { get; set; }
        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public int ArticleID { get; set; }
        [Required(ErrorMessage = "Please put packages.")]
        [Range(0, Int32.MaxValue)]
        public int QtyPackages { get; set; } = 0;
        [Display(Name = "Extra")]
        [Range(0, Int32.MaxValue)]
        public decimal QtyExtra { get; set; } = 0;

        public decimal TotalWeight { get; set; }
        public decimal TotalPrice { get; set; }

        public Guid? Guid { get; set; }

        //nav
        public Employee Manager { get; set; }
        public Company Company { get; set; }
        public Warehouse Warehouse { get; set; }
        public Article Article { get; set; }
        public IEnumerable<DicingPlanDetail> DicingPlanDetails { get; set; }
    }
}
