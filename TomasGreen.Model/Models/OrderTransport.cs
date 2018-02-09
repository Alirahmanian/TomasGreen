using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace TomasGreen.Model.Models
{
    public class OrderTransport :BaseEntity
    {
        [Required(ErrorMessage ="Name is requierd")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string Name { get; set; }
    }
}
