using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TomasGreen.Model.Models
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        //relations
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please choose a company.")]
        public int CompanyID { get; set; }
        [Display(Name = "Employee")]
        [Required(ErrorMessage = "Please choose an employee.")]
        public int EmployeeID { get; set; }
        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Please choose a currency.")]
        public int CurrencyID { get; set; }

        //Properties
        [Display(Name = "Order number")]
        public string OrderNumber { get; set; }
        [Display(Name = "Transport fee")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? TransportFee { get; set; } = 0;
        // [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Display(Name = "Total price")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? TotalPrice { get; set; } = 0;
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Order date")]
        public DateTime OrderDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "payment date")]
        public DateTime? PaymentDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Paid date")]
        public DateTime? PaidDate { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Loaded date")]
        public DateTime? LoadedDate { get; set; }
       // [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Display(Name = "Paid amount")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? AmountPaid { get; set; } = 0;
        public string Coments { get; set; }
        [Display(Name = "payment notes")]
        public string PaymentWarning { get; set; }
       
        [Display(Name = "Order is paid")]
        public bool OrderPaid { get; set; } = false;
        public bool Cash { get; set; } = false;
        public bool Confirmed { get; set; } = false;
        public bool HasIssue { get; set; }

        //Currency
       
       


        //nav.
       
        public Currency Currency { get; set; }
        public Company Company { get; set; }
        public Employee Employee { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

        public decimal GetTotalPrice()
        {
            decimal result = 0;
            foreach(var orderdetail in OrderDetails)
            {
                result += orderdetail.Extended_Price;
            }
            return result;
        }
    }
}