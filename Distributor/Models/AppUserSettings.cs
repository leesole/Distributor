using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.OfferEnums;
using static Distributor.Enums.OrderEnums;

namespace Distributor.Models
{
    public class AppUserSettings
    {
        [Key]
        public Guid AppUserSettingsId { get; set; }

        public Guid AppUserId { get; set; }


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
        [Display(Name = "Selection level")]
        public ExternalSearchLevelEnum CampaignDashboardExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Selection level")]
        public ExternalSearchLevelEnum RequiredListingDashboardExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Selection level")]
        public ExternalSearchLevelEnum AvailableListingDashboardExternalSelectionLevel { get; set; }


        [Display(Name = "Max distance from current branch (miles)")]
        public int? CampaignGeneralInfoMaxDistance { get; set; }

        [Display(Name = "Max distance from current branch (miles)")]
        public int? RequiredListingGeneralInfoMaxDistance { get; set; }

        [Display(Name = "Max distance from current branch (miles)")]
        public int? AvailableListingGeneralInfoMaxDistance { get; set; }


        [Required]
        [Display(Name = "Selection level")]
        public ExternalSearchLevelEnum CampaignGeneralInfoExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Selection level")]
        public ExternalSearchLevelEnum RequiredListingGeneralInfoExternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Selection level")]
        public ExternalSearchLevelEnum AvailableListingGeneralInfoExternalSelectionLevel { get; set; }


        [Required]
        [Display(Name = "Selection level")]
        public InternalSearchLevelEnum CampaignManageViewInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Selection level")]
        public InternalSearchLevelEnum AvailableListingManageViewInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Selection level")]
        public InternalSearchLevelEnum RequiredListingManageViewInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "Selection level")]
        public InternalSearchLevelEnum OffersManageViewInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "'Accepted' authorisation Level")]
        public InternalSearchLevelEnum OffersAcceptedAuthorisationManageViewLevel { get; set; }

        [Required]
        [Display(Name = "'Rejected' authorisation Level")]
        public InternalSearchLevelEnum OffersRejectedAuthorisationManageViewLevel { get; set; }

        [Required]
        [Display(Name = "'Returned' authorisation Level")]
        public InternalSearchLevelEnum OffersReturnedAuthorisationManageViewLevel { get; set; }

        [Required]
        [Display(Name = "Selection level")]
        public InternalSearchLevelEnum OrdersManageViewInternalSelectionLevel { get; set; }

        [Required]
        [Display(Name = "'Despatched' authorisation Level")]
        public InternalSearchLevelEnum OrdersDespatchedAuthorisationManageViewLevel { get; set; }

        [Required]
        [Display(Name = "'Delivered' authorisation Level")]
        public InternalSearchLevelEnum OrdersDeliveredAuthorisationManageViewLevel { get; set; }

        [Required]
        [Display(Name = "'Collected' authorisation Level")]
        public InternalSearchLevelEnum OrdersCollectedAuthorisationManageViewLevel { get; set; }

        [Required]
        [Display(Name = "'Closed' authorisation Level")]
        public InternalSearchLevelEnum OrdersClosedAuthorisationManageViewLevel { get; set; }
    }
}