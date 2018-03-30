using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class Currency :BaseEntity
    {
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Please put currency code.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please put rate.")]
        public decimal Rate { get; set; }
        [Display(Name = "Is base")]
        public bool IsBase { get; set; }
    }
}
