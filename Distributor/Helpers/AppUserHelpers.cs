using Distributor.Extenstions;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.Helpers
{
    public static class AppUserHelpers
    {
        #region Get

        public static AppUser GetAppUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUser appUser = GetAppUser(db, appUserId);
            db.Dispose();
            return appUser;
        }

        public static AppUser GetAppUser(ApplicationDbContext db, Guid appUserId)
        {
            return db.AppUsers.Find(appUserId);
        }

        public static AppUser GetAppUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUser appUser = GetAppUser(db, user);
            db.Dispose();
            return appUser;
        }

        public static AppUser GetAppUser(ApplicationDbContext db, IPrincipal user)
        {
            Guid appUserId;
            Guid.TryParse(user.Identity.GetAppUserId(), out appUserId);

            return GetAppUser(db, appUserId);
        }

        public static string GetAppUserName(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string name = GetAppUserName(db, appUserId);
            db.Dispose();
            return name;
        }

        public static string GetAppUserName(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = GetAppUser(db, appUserId);
            return appUser.FirstName + " " + appUser.LastName;
        }

        public static List<AppUser> GetManagerAppUsersForBranch(Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<AppUser> list = GetManagerAppUsersForBranch(db, branchId);
            db.Dispose();
            return list;
        }

        public static List<AppUser> GetManagerAppUsersForBranch(ApplicationDbContext db, Guid branchId)
        {
            List<AppUser> list = (from bu in db.BranchUsers
                                  join au in db.AppUsers on bu.UserId equals au.AppUserId
                                  where (bu.BranchId == branchId && bu.UserRole == UserRoleEnum.Manager)
                                  select au).Distinct().ToList();

            return list;
        }

        public static List<AppUser> GetAdminAppUsersForCompany(Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<AppUser> list = GetAdminAppUsersForCompany(db, companyId);
            db.Dispose();
            return list;
        }

        public static List<AppUser> GetAdminAppUsersForCompany(ApplicationDbContext db, Guid companyId)
        {
            List<AppUser> list = (from bu in db.BranchUsers
                                  join au in db.AppUsers on bu.UserId equals au.AppUserId
                                  where (bu.CompanyId == companyId && bu.UserRole == UserRoleEnum.Admin)
                                  select au).ToList();
            var listDistinct = list.Distinct();

            return list;
        }

        public static List<AppUser> GetAppUsersForBranch(Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<AppUser> list = GetAppUsersForBranch(db, branchId);
            db.Dispose();
            return list;
        }

        public static List<AppUser> GetAppUsersForBranch(ApplicationDbContext db, Guid branchId)
        {
            List<AppUser> list = (from bu in db.BranchUsers
                                  join au in db.AppUsers on bu.UserId equals au.AppUserId
                                  where (bu.BranchId == branchId)
                                  select au).Distinct().ToList();

            return list;
        }

        public static List<AppUser> GetAppUsersForCompany(Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<AppUser> list = GetAppUsersForCompany(db, companyId);
            db.Dispose();
            return list;
        }

        public static List<AppUser> GetAppUsersForCompany(ApplicationDbContext db, Guid companyId)
        {
            List<AppUser> list = (from bu in db.BranchUsers
                                  join au in db.AppUsers on bu.UserId equals au.AppUserId
                                  where (bu.CompanyId == companyId)
                                  select au).Distinct().ToList();

            return list;
        }

        public static List<AppUser> GetAllAppUsers()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<AppUser> list = GetAllAppUsers(db);
            db.Dispose();
            return list;
        }

        public static List<AppUser> GetAllAppUsers(ApplicationDbContext db)
        {
            return db.AppUsers.ToList();
        }

        public static List<AppUser> GetAllAppUsersForGroupForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<AppUser> list = GetAllAppUsersForGroupForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<AppUser> GetAllAppUsersForGroupForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<AppUser> list = GetAllAppUsers(db);

            //remove current user from list
            list.Remove(AppUserHelpers.GetAppUser(db, appUserId));

            return list;
        }

        #endregion

        #region Create

        public static AppUser CreateAppUser(string firstName, string lastName, Guid currentBranchId, EntityStatusEnum entityStatus, string loginEmail, PrivacyLevelEnum privacyLevel, UserRoleEnum userRole)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUser appUser = CreateAppUser(db, firstName, lastName, currentBranchId, entityStatus, loginEmail, privacyLevel, userRole);
            db.Dispose();
            return appUser;
        }

        public static AppUser CreateAppUser(ApplicationDbContext db, string firstName, string lastName, Guid currentBranchId, EntityStatusEnum entityStatus, string loginEmail, PrivacyLevelEnum privacyLevel, UserRoleEnum userRole)
        {
            AppUser appUser = new AppUser()
            {
                AppUserId = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                CurrentBranchId = currentBranchId,
                EntityStatus = entityStatus,
                PrivacyLevel = privacyLevel,
                LoginEmail = loginEmail
            };
            db.AppUsers.Add(appUser);

            //Create initial settings values from the settings template
            AppUserSettingsHelpers.CreateAppUserSettingsForNewUser(db, appUser.AppUserId, userRole);

            db.SaveChanges();

            return appUser;
        }

        #endregion

        #region Update

        public static AppUser UpdateCurrentBranchId(Guid appUserId, Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUser appUser = UpdateCurrentBranchId(db, appUserId, branchId);
            db.Dispose();
            return appUser;
        }

        public static AppUser UpdateCurrentBranchId(ApplicationDbContext db, Guid appUserId, Guid branchId)
        {
            AppUser appUser = GetAppUser(db, appUserId);
            appUser.CurrentBranchId = branchId;
            db.Entry(appUser).State = EntityState.Modified;
            db.SaveChanges();

            return appUser;
        }

        public static AppUser UpdateEntityStatus(Guid appUserId, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUser appUser = UpdateEntityStatus(db, appUserId, entityStatus);
            db.Dispose();
            return appUser;
        }

        public static AppUser UpdateEntityStatus(ApplicationDbContext db, Guid appUserId, EntityStatusEnum entityStatus)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            appUser.EntityStatus = entityStatus;
            db.Entry(appUser).State = EntityState.Modified;
            db.SaveChanges();

            return appUser;
        }

        public static AppUser UpdateAppUserExcludingCurrentBranchField(Guid appUserId, string firstName, string lastName, EntityStatusEnum entityStatus, PrivacyLevelEnum privacyLevel)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUser appUser = UpdateAppUserExcludingCurrentBranchField(db, appUserId, firstName, lastName, entityStatus, privacyLevel);
            db.Dispose();
            return appUser;
        }

        public static AppUser UpdateAppUserExcludingCurrentBranchField(ApplicationDbContext db, Guid appUserId, string firstName, string lastName, EntityStatusEnum entityStatus, PrivacyLevelEnum privacyLevel)
        {
            AppUser appUser = GetAppUser(db, appUserId);
            appUser.FirstName = firstName;
            appUser.LastName = lastName;
            appUser.EntityStatus = entityStatus;
            appUser.PrivacyLevel = privacyLevel;

            db.Entry(appUser).State = EntityState.Modified;
            db.SaveChanges();

            return appUser;
        }

        public static AppUser UpdateAppUserFromAppUserEditView(AppUserEditView view)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUser appUser = UpdateAppUserFromAppUserEditView(db, view);
            db.Dispose();
            return appUser;
        }

        public static AppUser UpdateAppUserFromAppUserEditView(ApplicationDbContext db, AppUserEditView view)
        {
            //Update AppUser
            AppUser appUser = GetAppUser(db, view.AppUserId);
            appUser.FirstName = view.FirstName;
            appUser.LastName = view.LastName;
            appUser.EntityStatus = view.EntityStatus;
            appUser.PrivacyLevel = view.PrivacyLevel;

            if (view.SelectedBranchId != null)
                appUser.CurrentBranchId = view.SelectedBranchId.Value;
            db.Entry(appUser).State = EntityState.Modified;
            db.SaveChanges();

            //Update BranchUser (Role)
            BranchUser branchUser = BranchUserHelpers.GetBranchUser(db, appUser.AppUserId, appUser.CurrentBranchId);
            BranchUserHelpers.UpdateBranchUserRole(db, branchUser.BranchUserId, view.UserRole);

            //Update UserSettings
            AppUserSettingsHelpers.UpdateUserSettingsFromAppUserEditView(db, view);

            return appUser;
        }

        #endregion

        #region Delete

        public static void DeleteAppUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            DeleteAppUser(db, appUserId);
            db.Dispose();
        }

        public static void DeleteAppUser(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            db.AppUsers.Remove(appUser);

            AppUserSettingsHelpers.DeleteAppUserSettingsForUser(db, appUserId);

            db.SaveChanges();
        }

        #endregion

        #region Processes

        public static bool IsAppUserActive(ApplicationUser user)
        {
            AppUser appUser = GetAppUser(user.AppUserId);

            if (appUser.EntityStatus == EntityStatusEnum.Active)
                return true;
            else
                return false;
        }

        public static EntityStatusEnum GetAppUserEntityStatus(ApplicationUser user)
        {
            AppUser appUser = GetAppUser(user.AppUserId);

            return appUser.EntityStatus;
        }

        //User.Identity.GetAppUserId returns Guid as a string, this converts that string back to a Guid
        public static Guid GetGuidFromUserGetAppUserId(string getAppUserId)
        {
            Guid appUserId;
            Guid.TryParse(getAppUserId, out appUserId);

            return appUserId;
        }

        public static Guid GetAppUserIdFromUser(IPrincipal user)
        {
            return GetGuidFromUserGetAppUserId(user.Identity.GetAppUserId());
        }

        #endregion
    }

    public static class AppUserEditViewHelpers
    {
        #region Get

        public static AppUserEditView GetAppUserEditViewForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserEditView view = GetAppUserEditViewForUser(db, user);
            db.Dispose();
            return view;
        }

        public static AppUserEditView GetAppUserEditViewForUser(ApplicationDbContext db, IPrincipal user)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            return GetAppUserEditViewForUser(db, appUser);
        }

        public static AppUserEditView GetAppUserEditViewForUser(AppUser appUserDetails)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserEditView view = GetAppUserEditViewForUser(db, appUserDetails);
            db.Dispose();
            return view;
        }

        public static AppUserEditView GetAppUserEditViewForUser(ApplicationDbContext db, AppUser appUserDetails)
        {
            Branch branch = BranchHelpers.GetBranch(db, appUserDetails.CurrentBranchId);
            BranchUser branchUser = BranchUserHelpers.GetBranchUser(db, appUserDetails.AppUserId, appUserDetails.CurrentBranchId);

            AppUserSettings appUserSettings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserDetails.AppUserId);

            AppUserEditView view = new AppUserEditView()
            {
                AppUserId = appUserDetails.AppUserId,
                FirstName = appUserDetails.FirstName,
                LastName = appUserDetails.LastName,
                EntityStatus = appUserDetails.EntityStatus,
                PrivacyLevel = appUserDetails.PrivacyLevel,
                SelectedBranchId = appUserDetails.CurrentBranchId,
                UserRole = branchUser.UserRole,
                AppUserSettingsId = appUserSettings.AppUserSettingsId,
                BranchName = branch.BranchName,
                BranchBusinessType = branch.BusinessType,
                BranchAddressLine1 = branch.AddressLine1,
                BranchAddressLine2 = branch.AddressLine2,
                BranchAddressLine3 = branch.AddressLine3,
                BranchAddressTownCity = branch.AddressTownCity,
                BranchAddressCounty = branch.AddressCounty,
                BranchAddressPostcode = branch.AddressPostcode,
                CampaignDashboardDisplayBlockedListings = appUserSettings.CampaignDashboardDisplayBlockedListings,
                CampaignDashboardMaxDistance = appUserSettings.CampaignDashboardMaxDistance,
                CampaignDashboardMaxAge = appUserSettings.CampaignDashboardMaxAge,
                RequiredListingDashboardDisplayBlockedListings = appUserSettings.RequiredListingDashboardDisplayBlockedListings,
                RequiredListingDashboardMaxDistance = appUserSettings.RequiredListingDashboardMaxDistance,
                RequiredListingDashboardMaxAge = appUserSettings.RequiredListingDashboardMaxAge,
                AvailableListingDashboardDisplayBlockedListings = appUserSettings.AvailableListingDashboardDisplayBlockedListings,
                AvailableListingDashboardMaxDistance = appUserSettings.AvailableListingDashboardMaxDistance,
                AvailableListingDashboardMaxAge = appUserSettings.AvailableListingDashboardMaxAge,
                CampaignDashboardExternalSelectionLevel = appUserSettings.CampaignDashboardExternalSelectionLevel,
                RequiredListingDashboardExternalSelectionLevel = appUserSettings.RequiredListingDashboardExternalSelectionLevel,
                AvailableListingDashboardExternalSelectionLevel = appUserSettings.AvailableListingDashboardExternalSelectionLevel,
                CampaignGeneralInfoDisplayBlockedListings = appUserSettings.CampaignGeneralInfoDisplayBlockedListings,
                CampaignGeneralInfoMaxDistance = appUserSettings.CampaignGeneralInfoMaxDistance,
                RequiredListingGeneralInfoDisplayBlockedListings = appUserSettings.RequiredListingGeneralInfoDisplayBlockedListings,
                RequiredListingGeneralInfoMaxDistance = appUserSettings.RequiredListingGeneralInfoMaxDistance,
                AvailableListingGeneralInfoDisplayBlockedListings = appUserSettings.AvailableListingGeneralInfoDisplayBlockedListings,
                AvailableListingGeneralInfoMaxDistance = appUserSettings.AvailableListingGeneralInfoMaxDistance,
                CampaignManageViewInternalSelectionLevel = appUserSettings.CampaignManageViewInternalSelectionLevel,
                RequiredListingManageViewInternalSelectionLevel = appUserSettings.RequiredListingManageViewInternalSelectionLevel,
                AvailableListingManageViewInternalSelectionLevel = appUserSettings.AvailableListingManageViewInternalSelectionLevel,
                OffersManageViewInternalSelectionLevel = appUserSettings.OffersManageViewInternalSelectionLevel,
                OffersAcceptedAuthorisationManageViewLevel = appUserSettings.OffersAcceptedAuthorisationManageViewLevel,
                OffersRejectedAuthorisationManageViewLevel = appUserSettings.OffersRejectedAuthorisationManageViewLevel,
                OffersReturnedAuthorisationManageViewLevel = appUserSettings.OffersReturnedAuthorisationManageViewLevel,
                OrdersManageViewInternalSelectionLevel = appUserSettings.OrdersManageViewInternalSelectionLevel,
                OrdersDespatchedAuthorisationManageViewLevel = appUserSettings.OrdersDespatchedAuthorisationManageViewLevel,
                OrdersDeliveredAuthorisationManageViewLevel = appUserSettings.OrdersDeliveredAuthorisationManageViewLevel,
                OrdersCollectedAuthorisationManageViewLevel = appUserSettings.OrdersCollectedAuthorisationManageViewLevel,
                OrdersClosedAuthorisationManageViewLevel = appUserSettings.OrdersClosedAuthorisationManageViewLevel,
                CampaignGeneralInfoExternalSelectionLevel = appUserSettings.CampaignGeneralInfoExternalSelectionLevel,
                RequiredListingGeneralInfoExternalSelectionLevel = appUserSettings.RequiredListingGeneralInfoExternalSelectionLevel,
                AvailableListingGeneralInfoExternalSelectionLevel = appUserSettings.AvailableListingGeneralInfoExternalSelectionLevel,
                GroupListViewsForUserOnly = GroupViewHelpers.GetGroupEditViewForForUserOnly(db, appUserDetails.AppUserId),
                UserBlockListView = BlockViewHelpers.GetBlockViewByType(db, appUserDetails.AppUserId, LevelEnum.User),
                UserBranchBlockListView = BlockViewHelpers.GetBlockViewByType(db, appUserDetails.AppUserId, LevelEnum.Branch),
                UserCompanyBlockListView = BlockViewHelpers.GetBlockViewByType(db, appUserDetails.AppUserId, LevelEnum.Company)
            };

            return view;
        }

        #endregion
    }
}