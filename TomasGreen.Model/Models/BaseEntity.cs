using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TomasGreen.Model.Models
{
   public class BaseEntity
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [ScaffoldColumn(false)]
        [DisplayName("Created by")]
        public DateTime AddedDate { get; set; } = DateTime.Now;
        [ScaffoldColumn(false)]
        [DisplayName("Changed by")]
        public DateTime? ModifiedDate { get; set; }
        [ScaffoldColumn(false)]
        [DisplayName("Saved by")]
        public string UserName { get; set; }
        public bool Archive { get; set; } = false;
        public Guid? Guid { get; set; }

    }
}
