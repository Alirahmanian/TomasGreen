using System;
using System.Collections.Generic;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class ArticleCategory :BaseEntity
    {
        public string Name { get; set; }
        public bool Archive { get; set; }

        //nav.
        public virtual ICollection<Article> Articles { get; set; }

    }
}
