using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CompanyCreditDebitBalance : BaseEntity
    {
        public int CompanyID { get; set; }
        public int CurrencyID { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Credit { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Debit { get; set; }

        //nav.
        public Company Company { get; set; }
        public Currency Currency { get; set; }
    }
}
