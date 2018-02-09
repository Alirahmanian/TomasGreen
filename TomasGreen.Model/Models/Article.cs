using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TomasGreen.Model.Models
{
    public class Article :BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public decimal MinimumPrice { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public decimal BoxWeight { get; set; }
       

        public Int64 ArticleCategoryID { get; set; }
        [Display(Name = "Article category")]

        public Int64 CountryID { get; set; }
        [Display(Name = "Country")]
        public Country Country { get; set; }


        //nav.
        public ArticleCategory ArticleCategory { get; set; }
        
    }
}