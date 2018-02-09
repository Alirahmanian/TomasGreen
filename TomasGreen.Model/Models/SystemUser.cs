using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class SystemUser  :BaseEntity
    {
        public int UserId { get; set; }
        public UserType UserType { get; set; }
        public int? CompanyID { get; set; }
        public Company Company { get; set; }
    }
    
}
