using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CompanyBalanceDetailType : BaseEntity
    {
        [Required(ErrorMessage = "Please put a baalance changer type.")]
        public string Name { get; set; }

        //nav.
        public ICollection<CompanyBalanceDetail> CompanyBalanceDetails { get; set; }
    }
}
