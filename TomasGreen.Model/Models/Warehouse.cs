using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TomasGreen.Model.Models
{
    public class Warehouse : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool Archive { get; set; }

        //nav
        public virtual IEnumerable<ReceiveArticleWarehouse> ReceivedArticles { get; set; }
      //  public virtual ArticleWarehouseBalance ArticleWarehouseBalance { get; set; }

    }
}