using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
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
        public EntityStatus EntityStatus { get; set; }

        //Company
        [Display(Name = "Comapny name")]
        public string CompanyName { get; set; }

        [Display(Name = "Comapny registration details")]
        public string CompanyRegistrationDetails { get; set; }

        [Display(Name = "Charity registration details")]
        public string CharityRegistrationDetails { get; set; }

        [Display(Name = "VAT registration details")]
        public string VATRegistrationDetails { get; set; }

        //Branch
        [Display(Name = "Branch name")]
        public string BranchName { get; set; }

        [Display(Name = "Address line 1")]
        public string BranchAddressLine1 { get; set; }

        [Display(Name = "Address line 2")]
        public string BranchAddressLine2 { get; set; }

        [Display(Name = "Address line 3")]
        public string BranchAddressLine3 { get; set; }

        [Display(Name = "Address town/city")]
        public string BranchAddressTownCity { get; set; }

        [Display(Name = "Address county")]
        public string BranchAddressCounty { get; set; }

        [Display(Name = "Address postcode")]
        public string BranchAddressPostcode { get; set; }

        [Display(Name = "Telephone number")]
        public string BranchTelephoneNumber { get; set; }

        [Display(Name = "Email")]
        public string BranchEmail { get; set; }

        [Display(Name = "Contact name")]
        public string BranchContactName { get; set; }
    }

    public class ResetPasswordViewModel
    {
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

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
