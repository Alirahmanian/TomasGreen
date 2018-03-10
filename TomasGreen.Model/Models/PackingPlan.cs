using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomasGreen.Model.Models
{
    public class PackingPlan : BaseEntity
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Display(Name = "Manager")]
        [Required(ErrorMessage = "Please choose a manager.")]
        public Int64 ManagerID { get; set; }
        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please choose a company.")]
        public Int64 CompanyID { get; set; }
        public Guid? Guid { get; set; }
        //nav
        public ICollection<PackingPlanMix> Mixes { get; set; }

    }
}
