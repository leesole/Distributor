using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Templates
{
    public class AppUserSettingsTemplate
    {
        public int? GlobalMaxDistance = null;
        public double? GlobalMaxAge = null;
        public InternalSearchLevelEnum GlobalInternalSelectionLevel = InternalSearchLevelEnum.User;
        public ExternalSearchLevelEnum GlobalExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public int? AvailableListingGeneralInfoMaxDistance = null;
        public int? AvailableListingRecentMaxDistance = null;
        public double? AvailableListingRecentMaxAge = null;
        public int? RequiredListingGeneralInfoMaxDistance = null;
        public int? RequiredListingRecentMaxDistance = null;
        public double? RequiredListingRecentMaxAge = null;
        public InternalSearchLevelEnum AvailableListingManageViewInternalSelectionLevel = InternalSearchLevelEnum.User;
        public InternalSearchLevelEnum RequiredListingManageViewInternalSelectionLevel = InternalSearchLevelEnum.User;
        public ExternalSearchLevelEnum AvailableListingGeneralInfoExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public ExternalSearchLevelEnum RequiredListingGeneralInfoExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public ExternalSearchLevelEnum AvailableListingRecentExternalSelectionLevel = ExternalSearchLevelEnum.All;
        public ExternalSearchLevelEnum RequiredListingRecentExternalSelectionLevel = ExternalSearchLevelEnum.All;
    }
}