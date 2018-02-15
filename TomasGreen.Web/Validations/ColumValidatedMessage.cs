using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TomasGreen.Web.Validations
{
    public class ColumValidatedMessage
    {
        public bool Value { get; set; }
        public string Property { get; set; }
        public string Message { get; set; }
        public ColumValidatedMessage(bool value, string property, string message)
        {
            Value = value;
            Property = property;
            Message = message;
            
        }
    }
}
