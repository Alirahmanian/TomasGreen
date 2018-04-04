using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CompanyCreditDebitBalanceDetailType : BaseEntity
    {
        [Required(ErrorMessage = "Please put a baalance changer type.")]
        public string Name { get; set; }
        public bool UsedBySystem { get; set; }

        //nav.
        public ICollection<CompanyCreditDebitBalanceDetail> CompanyCreditDebitBalanceDetails { get; set; }
    }
}
