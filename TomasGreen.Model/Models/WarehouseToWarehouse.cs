using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
   public class WarehouseToWarehouse : BaseEntity
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please choose a warehouse.")]
        [Display(Name = "Sender warehouse")]
        public int SenderWarehouseID { get; set; }
        [Required(ErrorMessage = "Please choose a warehouse.")]
        [Display(Name = "Recipient warehouse")]
        public int RecipientWarehouseID { get; set; }

        [Required(ErrorMessage = "Please choose a company.")]
        [Display(Name = "Sender company")]
        public int SenderCompanyID { get; set; }
        [Required(ErrorMessage = "Please choose a company.")]
        [Display(Name = "Recipient company")]
        public int RecipientCompanyID { get; set; }

        [Display(Name = "Article")]
        [Required(ErrorMessage = "Please choose an article.")]
        public int ArticleID { get; set; }

        [Display(Name = "Packages")]
        [Required(ErrorMessage = "Please put packages.")]
        [Range(0, Int32.MaxValue)]
        public int QtyPackages { get; set; } = 0;
        [Display(Name = "Extra")]
        [Range(0, Int32.MaxValue)]
        public decimal QtyExtra { get; set; } = 0;


        
    
    
    
}
}
