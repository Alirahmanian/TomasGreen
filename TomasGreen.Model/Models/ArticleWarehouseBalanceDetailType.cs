using System.Collections.Generic;

namespace TomasGreen.Model.Models
{
    public class ArticleWarehouseBalanceDetailType : BaseEntity
    {
        public string Name { get; set; }
        public bool UsedBySystem { get; set; }

        //nav.
        public ICollection<ArticleWarehouseBalanceDetail> ArticleWarehouseBalanceDetails { get; set; }
    }
}