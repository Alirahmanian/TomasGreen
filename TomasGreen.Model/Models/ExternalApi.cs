using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class ExternalApi : BaseEntity
    {
        [Required(ErrorMessage = "Please put name.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please put provider.")]
        public string Provider { get; set; }
        [Required(ErrorMessage = "Please put link.")]
        public string Link { get; set; }
        [Required(ErrorMessage = "Please put Key.")]
        public string Key { get; set; }

        //nav.
        public ICollection<ExternalApiFunction> Functions { get; set; }
    }
}
