using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CompanySection :BaseEntity
    {
        public int CompanyID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string Name { get; set; }

        //
        public Company Company { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<PurchaseArticle> Purchases { get; set; }
    }
}
