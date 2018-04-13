using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
   public class Payment : BaseEntity
   {
        public DateTime Date { get; set; }
        public int PayingCompanyID { get; set; }
        public int ReceivingCompanyID { get; set; }
    }
}
