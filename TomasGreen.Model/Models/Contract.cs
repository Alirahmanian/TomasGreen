using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class Contract : BaseEntity
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string ContractNumber { get; set; }
        public string Description { get; set; }
        [Display(Name = "Company")]
        public int CompanyID { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
