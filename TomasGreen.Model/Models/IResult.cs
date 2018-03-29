using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TomasGreen.Model.Models
{
    public interface IResult
    {
        bool Value { get; set; }
        string Action { get; set; }
        string Message { get; set; }
        string SystemMessage { get; set; }
    }
}
