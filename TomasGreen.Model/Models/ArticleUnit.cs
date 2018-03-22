using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class ArticleUnit : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Invalid string lenght")]
        public string Name { get; set; }
        [Display(Name = "G")]
        public bool MeasuresByG { get; set; }
        [Display(Name = "KG")]
        public bool MeasuresByKg { get; set; }
        [Display(Name = "TON")]
        public bool MeasuresByTon{ get; set; }
        [Display(Name = "Piece")]
        public bool MeasuresByPiece { get; set; }
        [Display(Name = "In kg")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal MeasurePerKg { get; set; } = 0;

        //nav.
        public ICollection<Article> Articles { get; set; }

        public PropertyValidation Validate()
        {
            var result = new PropertyValidation(true, "Validate", "ArticleUnit", "", "");
            var measures = 0;
            measures += (MeasuresByG == true) ? 1 : 0;
            measures += (MeasuresByKg == true) ? 1 : 0;
            measures += (MeasuresByTon == true) ? 1 : 0;
            measures += (MeasuresByPiece== true) ? 1 : 0;
            if(measures > 1)
            {
                result.Value = false; result.Property = "Measures by"; result.Message = "Please choose only one measurement.";
                return result;
            }
            if(Name == "")
            {
                result.Value = false; result.Property = nameof(Name); result.Message = "Please put name.";
                return result;
            }

            return result;
        }
        
    }
}
