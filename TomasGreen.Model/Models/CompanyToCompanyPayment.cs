using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
   public class CompanyToCompanyPayment : BaseEntity
   {
        public DateTime Date { get; set; }
        public int PayingCompanyID { get; set; }
        public int PaidCurrencyID { get; set; }
        public decimal PaidAmount { get; set; }
        public bool Cash { get; set; }
        public int CurrencyID { get; set; }
        public decimal Rate { get; set; }
        public decimal ExhangedAmount { get; set; }
        public decimal Discount { get; set; }
        public bool IsDiscountRate { get; set; }
        public int ReceivingCompanyID { get; set; }

        //nav.
        public Company PayingCompany { get; set; }
        public Company ReceivingCompany { get; set; }
        public Currency PaidCurrency { get; set; }
        public Currency Currency { get; set; }
    }
}
