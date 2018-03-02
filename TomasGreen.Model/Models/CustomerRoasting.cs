using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CustomerRoasting : BaseEntity
    {
        public Int64 CustomerID { get; set; }
        public DateTime DeliveringDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaidDate { get; set; }
        public decimal TotalPrice { get; set; }


    }
}
