using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Models
{
    public class AppUserSettings
    {
        [Key]
        public Guid AppUserSettingsId { get; set; }

        public Guid AppUserId { get; set; }

        [Display(Name = "Max distance from current branch (miles)")]
        public int? GlobalMaxDistance { get; set; }

        [Display(Name = "max age since listing added (days)")]
        public double? GlobalMaxAge { get; set; }

        [Required]
        [Display(Name = "Internal selection level")]
        public InternalSearchLevelEnum GlobalInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum GlobalExternalSelectionLevel { get; set; }


        [Display(Name = "Max distance from current branch (miles)")]
        public int? AvailableListingGeneralInfoMaxDistance { get; set; }

        [Display(Name = "Max distance from current branch (miles)")]
        public int? AvailableListingRecentMaxDistance { get; set; }
        
        [Display(Name = "max age since listing added (days)")]
        public double? AvailableListingRecentMaxAge { get; set; }


        [Display(Name = "Max distance from current branch (miles)")]
        public int? RequiredListingGeneralInfoMaxDistance { get; set; }

        [Display(Name = "Max distance from current branch (miles)")]
        public int? RequiredListingRecentMaxDistance { get; set; }

        [Display(Name = "max age since listing added (days)")]
        public double? RequiredListingRecentMaxAge { get; set; }


        [Required]
        [Display(Name = "Internal selection level")]
        public InternalSearchLevelEnum AvailableListingManageViewInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Internal selection level")]
        public InternalSearchLevelEnum RequiredListingManageViewInternalSelectionLevel { get; set; }


        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum AvailableListingGeneralInfoExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum RequiredListingGeneralInfoExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum AvailableListingRecentExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "External selection level")]
        public ExternalSearchLevelEnum RequiredListingRecentExternalSelectionLevel { get; set; }
    }
}