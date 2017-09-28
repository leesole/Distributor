using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.OfferEnums;

namespace Distributor.Templates
{
    public class AppUserSettingsUserTemplate
    {
        public int? CampaignDashboardMaxDistance = null;
        public double? CampaignDashboardMaxAge = 7;
        public int? RequiredListingDashboardMaxDistance = null;
        public double? RequiredListingDashboardMaxAge = 7;
        public int? AvailableListingDashboardMaxDistance = null;
        public double? AvailableListingDashboardMaxAge = 7;
        public ExternalSearchLevelEnum CampaignDashboardExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public ExternalSearchLevelEnum RequiredListingDashboardExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public ExternalSearchLevelEnum AvailableListingDashboardExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public bool CampaignGeneralInfoDisplayBlockedListings = true;
        public int? CampaignGeneralInfoMaxDistance = null;
        public bool RequiredListingGeneralInfoDisplayBlockedListings = true;
        public int? RequiredListingGeneralInfoMaxDistance = null;
        public bool AvailableListingGeneralInfoDisplayBlockedListings = true;
        public int? AvailableListingGeneralInfoMaxDistance = null;
        public InternalSearchLevelEnum CampaignManageViewInternalSelectionLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum RequiredListingManageViewInternalSelectionLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum AvailableListingManageViewInternalSelectionLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum OffersManageViewInternalSelectionLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum OffersAcceptedAuthorisationManageViewLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum OffersRejectedAuthorisationManageViewLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum OffersReturnedAuthorisationManageViewLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum OrdersManageViewInternalSelectionLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum OrdersDespatchedAuthorisationManageViewLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum OrdersDeliveredAuthorisationManageViewLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum OrdersCollectedAuthorisationManageViewLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum OrdersClosedAuthorisationManageViewLevel = InternalSearchLevelEnum.User;
        public ExternalSearchLevelEnum CampaignGeneralInfoExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public ExternalSearchLevelEnum RequiredListingGeneralInfoExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public ExternalSearchLevelEnum AvailableListingGeneralInfoExternalSelectionLevel = ExternalSearchLevelEnum.All;
    }

    public class AppUserSettingsManagerTemplate : AppUserSettingsUserTemplate
    {        
        public new InternalSearchLevelEnum CampaignManageViewInternalSelectionLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum RequiredListingManageViewInternalSelectionLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum AvailableListingManageViewInternalSelectionLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum OffersManageViewInternalSelectionLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum OffersAcceptedAuthorisationManageViewLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum OffersRejectedAuthorisationManageViewLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum OffersReturnedAuthorisationManageViewLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum OrdersManageViewInternalSelectionLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum OrdersDespatchedAuthorisationManageViewLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum OrdersDeliveredAuthorisationManageViewLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum OrdersCollectedAuthorisationManageViewLevel = InternalSearchLevelEnum.Branch;
        public new InternalSearchLevelEnum OrdersClosedAuthorisationManageViewLevel = InternalSearchLevelEnum.Branch;
    }

    public class AppUserSettingsAdminTemplate : AppUserSettingsUserTemplate
    {
        public new InternalSearchLevelEnum CampaignManageViewInternalSelectionLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum RequiredListingManageViewInternalSelectionLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum AvailableListingManageViewInternalSelectionLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum OffersManageViewInternalSelectionLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum OffersAcceptedAuthorisationManageViewLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum OffersRejectedAuthorisationManageViewLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum OffersReturnedAuthorisationManageViewLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum OrdersManageViewInternalSelectionLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum OrdersDespatchedAuthorisationManageViewLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum OrdersDeliveredAuthorisationManageViewLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum OrdersCollectedAuthorisationManageViewLevel = InternalSearchLevelEnum.Company;
        public new InternalSearchLevelEnum OrdersClosedAuthorisationManageViewLevel = InternalSearchLevelEnum.Company;
    }
}