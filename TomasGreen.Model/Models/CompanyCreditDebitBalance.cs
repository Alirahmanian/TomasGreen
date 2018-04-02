using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CompanyCreditDebitBalance : BaseEntity
    {
        public int CompanyID { get; set; }
        public int CurrencyID { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }

        //nav.
        public Company Company { get; set; }
        public Currency Currency { get; set; }
    }
}
