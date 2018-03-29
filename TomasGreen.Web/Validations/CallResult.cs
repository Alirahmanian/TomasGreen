using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Validations
{
    public class CallResult :IResult
    {
        public bool Value { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Message { get; set; }
        public string SystemMessage { get; set; }
        public CallResult()
        {

        }
        public CallResult(bool value, string action, string controller)
        {
            Value = value;
            Action = action;
            Controller = controller;
        }
    }
}
