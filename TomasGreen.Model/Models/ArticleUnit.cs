using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class ArticleUnit : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string Name { get; set; }
        public bool MeasuresByKg { get; set; }
        public bool MeasuresByG { get; set; }
        public bool MeasuresByTon{ get; set; }
    }
}
