using System.ComponentModel.DataAnnotations;

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

        
    }
}