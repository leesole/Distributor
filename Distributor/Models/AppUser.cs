using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.UserEnums;

namespace Distributor.Models
{
    public class AppUser
    {
        [Key]
        public Guid AppUserId { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "User role")]
        public UserRole Role { get; set; }
    }
}