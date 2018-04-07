using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Please put currency code.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please put rate.")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Rate { get; set; }
       
        [Display(Name = "Is base")]
        public bool IsBase { get; set; }
    }
}
