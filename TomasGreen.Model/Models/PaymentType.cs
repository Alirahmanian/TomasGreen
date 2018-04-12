using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class PaymentType :BaseEntity
    {
        public int? CompanyCreditDebitBalanceDetailTypeID { get; set; }
        public string Name { get; set; }
        public bool UsedBySystem { get; set; }
    }
}
