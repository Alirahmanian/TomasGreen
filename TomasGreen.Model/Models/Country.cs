using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TomasGreen.Model.Models
{
    public class Country : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string Name { get; set; }
        public bool Archive { get; set; }

        //nav.
        public ICollection<Article> Articles { get; set; }
    }
}