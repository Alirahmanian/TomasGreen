using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TomasGreen.Web.Validations
{
    public class PropertyValidatedMessage
    {
        public bool Value { get; set; }
        public string Action { get; set; }
        public string Model { get; set; }
        public string Property { get; set; }
        public string Message { get; set; }
        public PropertyValidatedMessage()
        {
                
        }
        public PropertyValidatedMessage(bool value, string action, string model, string property, string message)
        {
            Value = value;
            Action = action;
            Model = model;
            Property = property;
            Message = message;
            
        }

        public Dictionary<string, string> ConvertToList()
        {
            var result = new Dictionary<string, string>();
            result.Add(nameof(Value), Value.ToString());
            result.Add(nameof(Action), Action.ToString());
            result.Add(nameof(Model), Model.ToString());
            result.Add(nameof(Property), Property.ToString());
            result.Add(nameof(Message), Message.ToString());
            return result;
        }
    }
}
