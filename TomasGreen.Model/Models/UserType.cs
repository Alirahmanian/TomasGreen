using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
   public class UserType : BaseEntity
   {
        public string Name { get; set; }
        public string DbTableName { get; set; }
    }
}
