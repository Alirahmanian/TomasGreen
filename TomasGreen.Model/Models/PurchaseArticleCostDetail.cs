﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class PurchaseArticleCostDetail :BaseEntity
    {
        public int PurchaseArticleID { get; set; }
        [Required(ErrorMessage = "Please choose a cost type.")]
        public int PaymentTypeID { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please choose a company.")]
        public int CompanyID { get; set; }
        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Please choose a currency.")]
        public int CurrencyID { get; set; }
        public bool Cash { get; set; }
        public decimal Amount { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //nav.
        public PurchaseArticle PurchaseArticle { get; set; }
        public Company Company { get; set; }
        public Currency Currency { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
