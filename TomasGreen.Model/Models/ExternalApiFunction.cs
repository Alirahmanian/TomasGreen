using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class ExternalApiFunction : BaseEntity
    {
        public int ExternalApiID { get; set; }
        public string Name { get; set; }
        public string ParameterPattern { get; set; }

        //nav.
        public ExternalApi ExternalApi { get; set; }
    }
}
