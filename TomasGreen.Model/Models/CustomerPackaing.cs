
using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CustomerPackaging : BaseEntity
    {
        public int CompanyID { get; set; }
        public int ManagerID { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }

        //
    }
}
