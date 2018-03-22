using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TomasGreen.Model.Models
{
    public class PropertyValidation
    {
        public bool Value { get; set; }
        public string Action { get; set; }
        public string Model { get; set; }
        public string Property { get; set; }
        public string Message { get; set; }

        public PropertyValidation()
        {

        }
        public PropertyValidation(bool value, string action, string model, string property, string message)
        {
            Value = value;
            Action = action;
            Model = model;
            Property = property;
            Message = message;

        }

       
    }
}

