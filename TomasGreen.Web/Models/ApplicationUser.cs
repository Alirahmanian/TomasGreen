using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public  SystemUser SystemUserId { get; set; }
        public IEnumerable<SystemUser> SystemUsers { get; set; }
    }

    

    
}
