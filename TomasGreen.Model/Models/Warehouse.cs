using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace TomasGreen.Model.Models
{
    public class Warehouse : BaseEntity
    {
        [Display(Name ="Section")]
        public int? CompanySectionID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Is on the way")]
        public bool IsOnTheWay { get; set; } = false;
        [Display(Name = "Is customers")]
        public bool IsCustomers { get; set; } = false;


        //nav
        public virtual CompanySection Section { get; set; }
        public  ICollection<PurchasedArticleDetail> PurchasedArticleDetails { get; set; }
        public virtual ICollection<ArticleWarehouseBalance> ArticleWarehouseBalances { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}