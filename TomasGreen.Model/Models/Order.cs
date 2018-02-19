using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TomasGreen.Model.Models
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please choose a company.")]
        public Int64 CompanyID { get; set; }
        [Display(Name = "Employee")]
        [Required(ErrorMessage = "Please choose an employee.")]
        public Int64 EmployeeID { get; set; }
        [Display(Name = "Transport")]
        [Required(ErrorMessage = "Please choose a transport type.")]
        public Int64 OrderTransportID { get; set; }
        public int AmountArticle { get; set; } = 0;
        public int AmountReserve { get; set; } = 0;
       // [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Display(Name = "Total price")]
        public decimal? TotalPrice { get { return GetTotalPrice(); } } 
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
        [Display(Name = "Loading date")]
        public DateTime? LoadingDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Loaded date")]
        public DateTime? LoadedDate { get; set; }
       // [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Display(Name = "Paid amount")]
        public decimal? AmountPaid { get; set; } = 0;
        public string Coments { get; set; }
        public string OrderdBy { get; set; }
        [Display(Name = "payment notes")]
        public string PaymentWarning { get; set; }
        [Display(Name = "Forced date")]
        public bool ForcedPaid { get; set; }
        [Display(Name = "Order is paid")]
        public bool OrderPaid { get; set; }
        public bool Cash { get; set; }
        public bool Confirmed { get; set; }
        public bool Archive { get; set; } 


        public Company Company { get; set; }
        public Employee Employee { get; set; }
        public OrderTransport OrderTransport { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

        

        private decimal GetTotalPrice()
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