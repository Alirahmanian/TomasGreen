
using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CustomerPackaging : BaseEntity
    {
        public Int64 CompanyID { get; set; }
        public Int64 ManagerID { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }

        //
    }
}
