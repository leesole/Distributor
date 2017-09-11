using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.BranchEnums;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.GeneralEnums;
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

    public class AppUserEditView : BaseViewWithCallingFields
    {
        public Guid AppUserId { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        //BranchUser
        public EntityStatusEnum EntityStatus { get; set; }

        //Branch
        public Guid? SelectedBranchId { get; set; }

        //UserRole
        public UserRoleEnum UserRole { get; set; }


        public Guid AppUserSettingsId { get; set; }

        //Branch Details
        [Display(Name = "Branch name")]
        public string BranchName { get; set; }

        [Display(Name = "Business type")]
        public BusinessTypeEnum BranchBusinessType { get; set; }

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

        [Required]
        [Display(Name = "Address postcode")]
        public string BranchAddressPostcode { get; set; }


        [Display(Name = "Max distance from current branch (miles)")]
        public int? CampaignDashboardMaxDistance { get; set; }

        [Display(Name = "max age since listing added (days)")]
        public double? CampaignDashboardMaxAge { get; set; }

        [Display(Name = "Max distance from current branch (miles)")]
        public int? RequiredListingDashboardMaxDistance { get; set; }

        [Display(Name = "max age since listing added (days)")]
        public double? RequiredListingDashboardMaxAge { get; set; }

        [Display(Name = "Max distance from current branch (miles)")]
        public int? AvailableListingDashboardMaxDistance { get; set; }

        [Display(Name = "max age since listing added (days)")]
        public double? AvailableListingDashboardMaxAge { get; set; }

        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum CampaignDashboardExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum AvailableListingDashboardExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum RequiredListingDashboardExternalSelectionLevel { get; set; }


        [Display(Name = "Max distance from current branch (miles)")]
        public int? CampaignGeneralInfoMaxDistance { get; set; }

        [Display(Name = "Max distance from current branch (miles)")]
        public int? RequiredListingGeneralInfoMaxDistance { get; set; }

        [Display(Name = "Max distance from current branch (miles)")]
        public int? AvailableListingGeneralInfoMaxDistance { get; set; }


        [Required]
        [Display(Name = "Internal selection level")]
        public InternalSearchLevelEnum CampaignManageViewInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Internal selection level")]
        public InternalSearchLevelEnum AvailableListingManageViewInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Internal selection level")]
        public InternalSearchLevelEnum RequiredListingManageViewInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Internal selection level")]
        public InternalSearchLevelEnum OffersManageViewInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Internal selection level")]
        public InternalSearchLevelEnum OrdersManageViewInternalSelectionLevel { get; set; }


        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum CampaignGeneralInfoExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum RequiredListingGeneralInfoExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum AvailableListingGeneralInfoExternalSelectionLevel { get; set; }
    }
}