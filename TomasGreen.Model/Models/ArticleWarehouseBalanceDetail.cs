using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TomasGreen.Model.Models
{
   public class ArticleWarehouseBalanceDetail : BaseEntity
    {
        [Required]
        public int ArticleID { get; set; }
        [Required]
        public int WarehouseID { get; set; }
        [Required]
        public int CompanyID { get; set; }
        [Required]
        public int ArticleWarehouseBalanceDetailTypeID { get; set; }
        [Required]
        public int BalanceChangerID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int  QtyPackages { get; set; }
        public decimal QtyExtra { get; set; }
        public int QtyPackagesOnHandBeforeChange { get; set; }
        public decimal QtyExtraOnHandBeforeChange { get; set; }
        public string Comment { get; set; }
        public bool RowCreatedBySystem { get; set; }

        //nav.
        public ArticleWarehouseBalanceDetailType ArticleWarehouseBalanceDetailType { get; set; }
        public Article Article { get; set; }
        public Warehouse Warehouse { get; set; }
        public Company Company { get; set; }
       
    }
}
