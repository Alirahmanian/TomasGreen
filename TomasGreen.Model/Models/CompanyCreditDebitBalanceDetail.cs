﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class CompanyCreditDebitBalanceDetail : BaseEntity
    {
        public DateTime Date { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please choose a company.")]
        public int CompanyID { get; set; }
        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Please choose a currency.")]
        public int CurrencyID { get; set; }
        public bool Cash { get; set; }
        [Display(Name = "Balance Changer type")]
        [Required(ErrorMessage = "Please choose a balance Changer type.")]
        public int CompanyCreditDebitBalanceDetailTypeID { get; set; }
        public int BalanceChangerID { get; set; }
        public int PaymentTypeID { get; set; }
        [Column(TypeName ="decimal(18,4)")]
        public decimal Credit { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Debit { get; set; }
        public string Comment { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal CreditBeforeChange { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal DebitBeforeChange { get; set; }
        public bool RowCreatedBySystem { get; set; }

        //nav.
        public CompanyCreditDebitBalanceDetailType CompanyCreditDebitBalanceDetailType { get; set; }
        public Company Company { get; set; }
        public Currency Currency { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
