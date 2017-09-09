using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Templates
{
    public class AppUserSettingsTemplate
    {
        public int? RequiredListingDashboardMaxDistance = null;
        public double? RequiredListingDashboardMaxAge = 7;
        public int? AvailableListingDashboardMaxDistance = null;
        public double? AvailableListingDashboardMaxAge = 7;
        public ExternalSearchLevelEnum RequiredListingDashboardExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public ExternalSearchLevelEnum AvailableListingDashboardExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public int? RequiredListingGeneralInfoMaxDistance = null;
        public int? AvailableListingGeneralInfoMaxDistance = null;
        public InternalSearchLevelEnum RequiredListingManageViewInternalSelectionLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum AvailableListingManageViewInternalSelectionLevel = InternalSearchLevelEnum.User;
        public ExternalSearchLevelEnum RequiredListingGeneralInfoExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public ExternalSearchLevelEnum AvailableListingGeneralInfoExternalSelectionLevel = ExternalSearchLevelEnum.All;
    }
}