using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TomasGreen.Model.Models
{
    public class Employee : BaseEntity
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Invalid string lenght")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public int? Age { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}