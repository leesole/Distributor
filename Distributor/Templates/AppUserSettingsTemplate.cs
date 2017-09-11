using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.OfferEnums;

namespace Distributor.Templates
{
    public class AppUserSettingsTemplate
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
        public int? CampaignGeneralInfoMaxDistance = null;
        public int? RequiredListingGeneralInfoMaxDistance = null;
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
}