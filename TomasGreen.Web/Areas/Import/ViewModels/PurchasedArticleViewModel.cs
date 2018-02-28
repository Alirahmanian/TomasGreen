using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Import.ViewModels
{
    public class PurchasedArticleViewModel
    {
        public Int64 ID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public  Article Article { get; set; }
        public  Company Company { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Received { get; set; }
        public decimal TotalPerUnit { get; set; }
        public DateTime? ExpectedToArrive { get; set; }
        public string WarehouseSummary { get; set; }
        public Dictionary<string, decimal> Warehouses { get; set; }
    }
}
