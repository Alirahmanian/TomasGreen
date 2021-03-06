﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace TomasGreen.Model.Models
{
    public class Company : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string Name { get; set; }
        public decimal Balance { get; set; } = 0;
        public bool Ruble { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal Purchases { get; set; } = 0;
        [Display(Name = "Sold to us")]
        public decimal SouldToUs { get; set; } = 0;
        public decimal Paid { get; set; } = 0;
        public decimal Received { get; set; } = 0;
        [Display(Name = "Last balance")]
        public decimal LastBalance { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last balance date")]
        public DateTime LastBalanceDate { get; set; }
        public bool Locked { get; set; }
        [Display(Name = "Received credit")]
        public decimal CreditReceived { get; set; } = 0;
        [Display(Name = "Credit limit")]
        public decimal CreditLimit { get; set; } = 0;
        public bool IsOwner { get; set; } = false;


        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<CompanySection> Sections { get; set; }
    }
}
