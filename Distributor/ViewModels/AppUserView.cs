using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.BranchEnums;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.ViewModels
{
    public class AppUserView
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //BranchUser
        public EntityStatusEnum EntityStatus { get; set; }

        //Branch
        public Guid? SelectedBranchId { get; set; }

        //UserRole
        public UserRoleEnum UserRole { get; set; }
    }
}