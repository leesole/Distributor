using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Helpers
{
    public static class DataHelpers
    {
        /// <summary>
        /// Gets the relevant Guid for the level of internal authorisation sent
        /// i.e. If this is a 'User' level of authorisation, then the current user AppUserId is returned.
        /// </summary>
        /// <param name="authorisationLevel"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Guid GetAuthorisationId(InternalSearchLevelEnum authorisationLevel, IPrincipal user)
        {
            Guid id = Guid.Empty;

            AppUser appUser = AppUserHelpers.GetAppUser(user);
            Branch branch = BranchHelpers.GetBranch(appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(appUser.AppUserId);

            switch (settings.OrdersDespatchedAuthorisationManageViewLevel)
            {
                case InternalSearchLevelEnum.User:
                    id = appUser.AppUserId;
                    break;
                case InternalSearchLevelEnum.Branch:
                    id = branch.BranchId;
                    break;
                case InternalSearchLevelEnum.Company:
                    id = branch.CompanyId;
                    break;
                case InternalSearchLevelEnum.Group: //TO BE DONE LSLSLS
                    id = appUser.AppUserId;
                    break;
            }

            return id;
        }
    }
}