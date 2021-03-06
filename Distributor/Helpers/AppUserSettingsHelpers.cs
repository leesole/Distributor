﻿using Distributor.Models;
using Distributor.Templates;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.Helpers
{
    public static class AppUserSettingsHelpers
    {
        #region Get

        public static AppUserSettings GetAppUserSettings(Guid appUserSettingsId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserSettings appUserSettings = GetAppUserSettings(db, appUserSettingsId);
            db.Dispose();
            return appUserSettings;
        }

        public static AppUserSettings GetAppUserSettings(ApplicationDbContext db, Guid appUserSettingsId)
        {
            return db.AppUserSettings.Find(appUserSettingsId);
        }

        public static AppUserSettings GetAppUserSettingsForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserSettings appUserSettings = GetAppUserSettingsForUser(db, appUserId);
            db.Dispose();
            return appUserSettings;
        }

        public static AppUserSettings GetAppUserSettingsForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserSettings appUserSettings = GetAppUserSettingsForUser(db, AppUserHelpers.GetAppUserIdFromUser(user));
            db.Dispose();
            return appUserSettings;
        }

        public static AppUserSettings GetAppUserSettingsForUser(ApplicationDbContext db, IPrincipal user)
        {
            AppUserSettings appUserSettings = GetAppUserSettingsForUser(db, AppUserHelpers.GetAppUserIdFromUser(user));
            return appUserSettings;
        }

        public static AppUserSettings GetAppUserSettingsForUser(ApplicationDbContext db, Guid appUserId)
        {
            AppUserSettings settings = (from s in db.AppUserSettings
                                        where s.AppUserId == appUserId
                                        select s).FirstOrDefault();

            return settings;
        }

        #endregion

        #region Create

        public static AppUserSettings CreateAppUserSettings(Guid appUserId,
            int? campaignDashboardMaxDistance,
            double? campaignDashboardMaxAge,
            int? requiredListingDashboardMaxDistance,
            double? requiredListingDashboardMaxAge,
            int? availableListingDashboardMaxDistance,
            double? availableListingDashboardMaxAge,
            ExternalSearchLevelEnum campaignDashboardExternalSelectionLevel,
            ExternalSearchLevelEnum requiredListingDashboardtExternalSelectionLevel,
            ExternalSearchLevelEnum availableListingDashboardExternalSelectionLevel,
            bool campaignGeneralInfoDisplayMyUserListings,
            bool campaignGeneralInfoDisplayMyBranchListings,
            bool campaignGeneralInfoDisplayMyCompanyListings,
            bool campaignGeneralInfoDisplayBlockedListings,
            int? campaignGeneralInfoMaxDistance,
            bool requiredListingGeneralInfoDisplayMyUserListings,
            bool requiredListingGeneralInfoDisplayMyBranchListings,
            bool requiredListingGeneralInfoDisplayMyCompanyListings,
            bool requiredListingGeneralInfoDisplayBlockedListings,
            int? requiredListingGeneralInfoMaxDistance,
            bool availableListingGeneralInfoDisplayMyUserListings,
            bool availableListingGeneralInfoDisplayMyBranchListings,
            bool availableListingGeneralInfoDisplayMyCompanyListings,
            bool availableListingGeneralInfoDisplayBlockedListings,
            int? availableListingGeneralInfoMaxDistance,
            ExternalSearchLevelEnum campaignGeneralInfoExternalSelectionLevel,
            ExternalSearchLevelEnum requiredListingGeneralInfoExternalSelectionLevel,
            ExternalSearchLevelEnum availableListingGeneralInfoExternalSelectionLevel,
            InternalSearchLevelEnum campaignManageViewInternalSelectionLevel,
            InternalSearchLevelEnum requiredListingManageViewInternalSelectionLevel,
            InternalSearchLevelEnum availableListingManageViewInternalSelectionLevel,
            InternalSearchLevelEnum offersManageViewInternalSelectionLevel,
            InternalSearchLevelEnum offersAcceptedAuthorisationManageViewLevel,
            InternalSearchLevelEnum offersRejectedAuthorisationManageViewLevel,
            InternalSearchLevelEnum offersReturnedAuthorisationManageViewLevel,
            InternalSearchLevelEnum ordersManageViewInternalSelectionLevel,
            InternalSearchLevelEnum ordersDespatchedAuthorisationManageViewLevel,
            InternalSearchLevelEnum ordersDeliveredAuthorisationManageViewLevel,
            InternalSearchLevelEnum ordersReceivedAuthorisationManageViewLevel,
            InternalSearchLevelEnum ordersCollectedAuthorisationManageViewLevel,
            InternalSearchLevelEnum ordersClosedAuthorisationManageViewLevel)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserSettings appUserSettings = CreateAppUserSettings(db, 
                appUserId, 
                campaignDashboardMaxDistance, 
                campaignDashboardMaxAge, 
                requiredListingDashboardMaxDistance, 
                requiredListingDashboardMaxAge, 
                availableListingDashboardMaxDistance, 
                availableListingDashboardMaxAge, 
                campaignDashboardExternalSelectionLevel, 
                requiredListingDashboardtExternalSelectionLevel, 
                availableListingDashboardExternalSelectionLevel,
                campaignGeneralInfoDisplayMyUserListings,
                campaignGeneralInfoDisplayMyBranchListings,
                campaignGeneralInfoDisplayMyCompanyListings,
                campaignGeneralInfoDisplayBlockedListings, 
                campaignGeneralInfoMaxDistance,
                requiredListingGeneralInfoDisplayMyUserListings,
                requiredListingGeneralInfoDisplayMyBranchListings,
                requiredListingGeneralInfoDisplayMyCompanyListings,
                requiredListingGeneralInfoDisplayBlockedListings, 
                requiredListingGeneralInfoMaxDistance,
                availableListingGeneralInfoDisplayMyUserListings,
                availableListingGeneralInfoDisplayMyBranchListings,
                availableListingGeneralInfoDisplayMyCompanyListings,
                availableListingGeneralInfoDisplayBlockedListings, 
                availableListingGeneralInfoMaxDistance, 
                campaignGeneralInfoExternalSelectionLevel, 
                requiredListingGeneralInfoExternalSelectionLevel, 
                availableListingGeneralInfoExternalSelectionLevel, 
                campaignManageViewInternalSelectionLevel, 
                requiredListingManageViewInternalSelectionLevel, 
                availableListingManageViewInternalSelectionLevel, 
                offersManageViewInternalSelectionLevel, 
                offersAcceptedAuthorisationManageViewLevel, 
                offersRejectedAuthorisationManageViewLevel, 
                offersReturnedAuthorisationManageViewLevel, 
                ordersManageViewInternalSelectionLevel, 
                ordersDespatchedAuthorisationManageViewLevel, 
                ordersDeliveredAuthorisationManageViewLevel,
                ordersReceivedAuthorisationManageViewLevel,
                ordersCollectedAuthorisationManageViewLevel, 
                ordersClosedAuthorisationManageViewLevel);
            db.Dispose();
            return appUserSettings;
        }

        public static AppUserSettings CreateAppUserSettings(ApplicationDbContext db, Guid appUserId,
            int? campaignDashboardMaxDistance,
            double? campaignDashboardMaxAge,
            int? requiredListingDashboardMaxDistance,
            double? requiredListingDashboardMaxAge,
            int? availableListingDashboardMaxDistance,
            double? availableListingDashboardMaxAge,
            ExternalSearchLevelEnum campaignDashboardExternalSelectionLevel,
            ExternalSearchLevelEnum requiredListingDashboardExternalSelectionLevel,
            ExternalSearchLevelEnum availableListingDashboardExternalSelectionLevel,
            bool campaignGeneralInfoDisplayMyUserListings,
            bool campaignGeneralInfoDisplayMyBranchListings,
            bool campaignGeneralInfoDisplayMyCompanyListings,
            bool campaignGeneralInfoDisplayBlockedListings,
            int? campaignGeneralInfoMaxDistance,
            bool requiredListingGeneralInfoDisplayMyUserListings,
            bool requiredListingGeneralInfoDisplayMyBranchListings,
            bool requiredListingGeneralInfoDisplayMyCompanyListings,
            bool requiredListingGeneralInfoDisplayBlockedListings,
            int? requiredListingGeneralInfoMaxDistance,
            bool availableListingGeneralInfoDisplayMyUserListings,
            bool availableListingGeneralInfoDisplayMyBranchListings,
            bool availableListingGeneralInfoDisplayMyCompanyListings,
            bool availableListingGeneralInfoDisplayBlockedListings,
            int? availableListingGeneralInfoMaxDistance,
            ExternalSearchLevelEnum campaignGeneralInfoExternalSelectionLevel,
            ExternalSearchLevelEnum requiredListingGeneralInfoExternalSelectionLevel,
            ExternalSearchLevelEnum availableListingGeneralInfoExternalSelectionLevel,
            InternalSearchLevelEnum campaignManageViewInternalSelectionLevel,
            InternalSearchLevelEnum requiredListingManageViewInternalSelectionLevel,
            InternalSearchLevelEnum availableListingManageViewInternalSelectionLevel,
            InternalSearchLevelEnum offersManageViewInternalSelectionLevel,
            InternalSearchLevelEnum offersAcceptedAuthorisationManageViewLevel,
            InternalSearchLevelEnum offersRejectedAuthorisationManageViewLevel,
            InternalSearchLevelEnum offersReturnedAuthorisationManageViewLevel,
            InternalSearchLevelEnum ordersManageViewInternalSelectionLevel,
            InternalSearchLevelEnum ordersDespatchedAuthorisationManageViewLevel,
            InternalSearchLevelEnum ordersDeliveredAuthorisationManageViewLevel,
            InternalSearchLevelEnum ordersReceivedAuthorisationManageViewLevel,
            InternalSearchLevelEnum ordersCollectedAuthorisationManageViewLevel,
            InternalSearchLevelEnum ordersClosedAuthorisationManageViewLevel)
        {
            AppUserSettings appUserSettings = new AppUserSettings()
            {
                AppUserSettingsId = Guid.NewGuid(),
                AppUserId = appUserId,
                CampaignDashboardMaxDistance = campaignDashboardMaxDistance,
                CampaignDashboardMaxAge = campaignDashboardMaxAge,
                RequiredListingDashboardMaxDistance = requiredListingDashboardMaxDistance,
                RequiredListingDashboardMaxAge = requiredListingDashboardMaxAge,
                AvailableListingDashboardMaxDistance = availableListingDashboardMaxDistance,
                AvailableListingDashboardMaxAge = availableListingDashboardMaxAge,
                CampaignDashboardExternalSelectionLevel = campaignDashboardExternalSelectionLevel,
                RequiredListingDashboardExternalSelectionLevel = requiredListingDashboardExternalSelectionLevel,
                AvailableListingDashboardExternalSelectionLevel = availableListingDashboardExternalSelectionLevel,
                CampaignGeneralInfoDisplayMyUserListings = campaignGeneralInfoDisplayMyUserListings,
                CampaignGeneralInfoDisplayMyBranchListings = campaignGeneralInfoDisplayMyBranchListings,
                CampaignGeneralInfoDisplayMyCompanyListings = campaignGeneralInfoDisplayMyCompanyListings,
                CampaignGeneralInfoDisplayBlockedListings = campaignGeneralInfoDisplayBlockedListings,
                CampaignGeneralInfoMaxDistance = campaignGeneralInfoMaxDistance,
                RequiredListingGeneralInfoDisplayMyUserListings = requiredListingGeneralInfoDisplayMyUserListings,
                RequiredListingGeneralInfoDisplayMyBranchListings = requiredListingGeneralInfoDisplayMyBranchListings,
                RequiredListingGeneralInfoDisplayMyCompanyListings = requiredListingGeneralInfoDisplayMyCompanyListings,
                RequiredListingGeneralInfoDisplayBlockedListings = requiredListingGeneralInfoDisplayBlockedListings,
                RequiredListingGeneralInfoMaxDistance = requiredListingGeneralInfoMaxDistance,
                AvailableListingGeneralInfoDisplayMyUserListings = availableListingGeneralInfoDisplayMyUserListings,
                AvailableListingGeneralInfoDisplayMyBranchListings = availableListingGeneralInfoDisplayMyBranchListings,
                AvailableListingGeneralInfoDisplayMyCompanyListings = availableListingGeneralInfoDisplayMyCompanyListings,
                AvailableListingGeneralInfoDisplayBlockedListings = availableListingGeneralInfoDisplayBlockedListings,
                AvailableListingGeneralInfoMaxDistance = availableListingGeneralInfoMaxDistance,
                CampaignGeneralInfoExternalSelectionLevel = campaignGeneralInfoExternalSelectionLevel,
                RequiredListingGeneralInfoExternalSelectionLevel = requiredListingGeneralInfoExternalSelectionLevel,
                AvailableListingGeneralInfoExternalSelectionLevel = availableListingGeneralInfoExternalSelectionLevel,
                CampaignManageViewInternalSelectionLevel = campaignManageViewInternalSelectionLevel,
                RequiredListingManageViewInternalSelectionLevel = requiredListingManageViewInternalSelectionLevel,
                AvailableListingManageViewInternalSelectionLevel = availableListingManageViewInternalSelectionLevel,
                OffersManageViewInternalSelectionLevel = offersManageViewInternalSelectionLevel,
                OffersAcceptedAuthorisationManageViewLevel = offersAcceptedAuthorisationManageViewLevel,
                OffersRejectedAuthorisationManageViewLevel = offersRejectedAuthorisationManageViewLevel,
                OffersReturnedAuthorisationManageViewLevel = offersReturnedAuthorisationManageViewLevel,
                OrdersManageViewInternalSelectionLevel = ordersManageViewInternalSelectionLevel,
                OrdersDespatchedAuthorisationManageViewLevel = ordersDespatchedAuthorisationManageViewLevel,
                OrdersDeliveredAuthorisationManageViewLevel = ordersDeliveredAuthorisationManageViewLevel,
                OrdersReceivedAuthorisationManageViewLevel = ordersReceivedAuthorisationManageViewLevel,
                OrdersCollectedAuthorisationManageViewLevel = ordersCollectedAuthorisationManageViewLevel,
                OrdersClosedAuthorisationManageViewLevel = ordersClosedAuthorisationManageViewLevel
            };
            db.AppUserSettings.Add(appUserSettings);
            db.SaveChanges();

            return appUserSettings;
        }

        public static AppUserSettings CreateAppUserSettingsForNewUser(Guid appUserId, UserRoleEnum userRole)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserSettings appUserSettings = CreateAppUserSettingsForNewUser(db, appUserId, userRole);
            db.Dispose();
            return appUserSettings;
        }

        public static AppUserSettings CreateAppUserSettingsForNewUser(ApplicationDbContext db, Guid appUserId, UserRoleEnum userRole)
        {
            AppUserSettingsUserTemplate template = new AppUserSettingsUserTemplate();

            //Use the USER template as the standard but override for Admin and Managers
            switch (userRole)
            {
                case UserRoleEnum.SuperUser:
                case UserRoleEnum.Admin:
                    template = new AppUserSettingsAdminTemplate();
                    break;
                case UserRoleEnum.Manager:
                    template = new AppUserSettingsManagerTemplate();
                    break;
            }

            AppUserSettings appUserSettings = new AppUserSettings()
            {
                AppUserSettingsId = Guid.NewGuid(),
                AppUserId = appUserId,
                CampaignDashboardMaxDistance = template.CampaignDashboardMaxDistance,
                CampaignDashboardMaxAge = template.CampaignDashboardMaxAge,
                RequiredListingDashboardMaxDistance = template.RequiredListingDashboardMaxDistance,
                RequiredListingDashboardMaxAge = template.RequiredListingDashboardMaxAge,
                AvailableListingDashboardMaxDistance = template.AvailableListingDashboardMaxDistance,
                AvailableListingDashboardMaxAge = template.AvailableListingDashboardMaxAge,
                CampaignDashboardExternalSelectionLevel = template.CampaignDashboardExternalSelectionLevel,
                RequiredListingDashboardExternalSelectionLevel = template.RequiredListingDashboardExternalSelectionLevel,
                AvailableListingDashboardExternalSelectionLevel = template.AvailableListingDashboardExternalSelectionLevel,
                CampaignGeneralInfoDisplayMyUserListings = template.CampaignGeneralInfoDisplayMyUserListings,
                CampaignGeneralInfoDisplayMyBranchListings = template.CampaignGeneralInfoDisplayMyBranchListings,
                CampaignGeneralInfoDisplayMyCompanyListings = template.CampaignGeneralInfoDisplayMyCompanyListings,
                CampaignGeneralInfoDisplayBlockedListings = template.CampaignGeneralInfoDisplayBlockedListings,
                CampaignGeneralInfoMaxDistance = template.CampaignGeneralInfoMaxDistance,
                RequiredListingGeneralInfoDisplayMyUserListings = template.RequiredListingGeneralInfoDisplayMyUserListings,
                RequiredListingGeneralInfoDisplayMyBranchListings = template.RequiredListingGeneralInfoDisplayMyBranchListings,
                RequiredListingGeneralInfoDisplayMyCompanyListings = template.RequiredListingGeneralInfoDisplayMyCompanyListings,
                RequiredListingGeneralInfoDisplayBlockedListings = template.RequiredListingGeneralInfoDisplayBlockedListings,
                RequiredListingGeneralInfoMaxDistance = template.RequiredListingGeneralInfoMaxDistance,
                AvailableListingGeneralInfoDisplayMyUserListings = template.AvailableListingGeneralInfoDisplayMyUserListings,
                AvailableListingGeneralInfoDisplayMyBranchListings = template.AvailableListingGeneralInfoDisplayMyBranchListings,
                AvailableListingGeneralInfoDisplayMyCompanyListings = template.AvailableListingGeneralInfoDisplayMyCompanyListings,
                AvailableListingGeneralInfoDisplayBlockedListings = template.AvailableListingGeneralInfoDisplayBlockedListings,
                AvailableListingGeneralInfoMaxDistance = template.AvailableListingGeneralInfoMaxDistance,
                CampaignManageViewInternalSelectionLevel = template.CampaignManageViewInternalSelectionLevel,
                RequiredListingManageViewInternalSelectionLevel = template.RequiredListingManageViewInternalSelectionLevel,
                AvailableListingManageViewInternalSelectionLevel = template.AvailableListingManageViewInternalSelectionLevel,
                OffersManageViewInternalSelectionLevel = template.OffersManageViewInternalSelectionLevel,
                OffersAcceptedAuthorisationManageViewLevel = template.OffersAcceptedAuthorisationManageViewLevel,
                OffersRejectedAuthorisationManageViewLevel = template.OffersRejectedAuthorisationManageViewLevel,
                OffersReturnedAuthorisationManageViewLevel = template.OffersReturnedAuthorisationManageViewLevel,
                OrdersManageViewInternalSelectionLevel = template.OrdersManageViewInternalSelectionLevel,
                OrdersDespatchedAuthorisationManageViewLevel = template.OrdersDespatchedAuthorisationManageViewLevel,
                OrdersDeliveredAuthorisationManageViewLevel = template.OrdersDeliveredAuthorisationManageViewLevel,
                OrdersReceivedAuthorisationManageViewLevel = template.OrdersReceivedAuthorisationManageViewLevel,
                OrdersCollectedAuthorisationManageViewLevel = template.OrdersCollectedAuthorisationManageViewLevel,
                OrdersClosedAuthorisationManageViewLevel = template.OrdersClosedAuthorisationManageViewLevel,
                CampaignGeneralInfoExternalSelectionLevel = template.CampaignGeneralInfoExternalSelectionLevel,
                RequiredListingGeneralInfoExternalSelectionLevel = template.RequiredListingGeneralInfoExternalSelectionLevel,
                AvailableListingGeneralInfoExternalSelectionLevel = template.AvailableListingGeneralInfoExternalSelectionLevel
            };
            db.AppUserSettings.Add(appUserSettings);
            db.SaveChanges();

            return appUserSettings;
        }

        #endregion

        #region Update

        public static AppUserSettings UpdateUserSettingsFromAppUserEditView(AppUserEditView view)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserSettings settings = UpdateUserSettingsFromAppUserEditView(db, view);
            db.Dispose();
            return settings;
        }

        public static AppUserSettings UpdateUserSettingsFromAppUserEditView(ApplicationDbContext db, AppUserEditView view)
        {
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettings(db, view.AppUserSettingsId);
            settings.CampaignDashboardMaxDistance = view.CampaignDashboardMaxDistance;
            settings.CampaignDashboardMaxAge = view.CampaignDashboardMaxAge;
            settings.RequiredListingDashboardMaxDistance = view.RequiredListingDashboardMaxDistance;
            settings.RequiredListingDashboardMaxAge = view.RequiredListingDashboardMaxAge;
            settings.AvailableListingDashboardMaxDistance = view.AvailableListingDashboardMaxDistance;
            settings.AvailableListingDashboardMaxAge = view.AvailableListingDashboardMaxAge;
            settings.CampaignDashboardExternalSelectionLevel = view.CampaignDashboardExternalSelectionLevel;
            settings.RequiredListingDashboardExternalSelectionLevel = view.RequiredListingDashboardExternalSelectionLevel;
            settings.AvailableListingDashboardExternalSelectionLevel = view.AvailableListingDashboardExternalSelectionLevel;
            settings.CampaignGeneralInfoDisplayMyUserListings = view.CampaignGeneralInfoDisplayMyUserListings;
            settings.CampaignGeneralInfoDisplayMyBranchListings = view.CampaignGeneralInfoDisplayMyBranchListings;
            settings.CampaignGeneralInfoDisplayMyCompanyListings = view.CampaignGeneralInfoDisplayMyCompanyListings;
            settings.CampaignGeneralInfoDisplayBlockedListings = view.CampaignGeneralInfoDisplayBlockedListings;
            settings.CampaignGeneralInfoMaxDistance = view.CampaignGeneralInfoMaxDistance;
            settings.RequiredListingGeneralInfoDisplayMyUserListings = view.RequiredListingGeneralInfoDisplayMyUserListings;
            settings.RequiredListingGeneralInfoDisplayMyBranchListings = view.RequiredListingGeneralInfoDisplayMyBranchListings;
            settings.RequiredListingGeneralInfoDisplayMyCompanyListings = view.RequiredListingGeneralInfoDisplayMyCompanyListings;
            settings.RequiredListingGeneralInfoDisplayBlockedListings = view.RequiredListingGeneralInfoDisplayBlockedListings;
            settings.RequiredListingGeneralInfoMaxDistance = view.RequiredListingGeneralInfoMaxDistance;
            settings.AvailableListingGeneralInfoDisplayMyUserListings = view.AvailableListingGeneralInfoDisplayMyUserListings;
            settings.AvailableListingGeneralInfoDisplayMyBranchListings = view.AvailableListingGeneralInfoDisplayMyBranchListings;
            settings.AvailableListingGeneralInfoDisplayMyCompanyListings = view.AvailableListingGeneralInfoDisplayMyCompanyListings;
            settings.AvailableListingGeneralInfoDisplayBlockedListings = view.AvailableListingGeneralInfoDisplayBlockedListings;
            settings.AvailableListingGeneralInfoMaxDistance = view.AvailableListingGeneralInfoMaxDistance;
            settings.CampaignManageViewInternalSelectionLevel = view.CampaignManageViewInternalSelectionLevel;
            settings.RequiredListingManageViewInternalSelectionLevel = view.RequiredListingManageViewInternalSelectionLevel;
            settings.AvailableListingManageViewInternalSelectionLevel = view.AvailableListingManageViewInternalSelectionLevel;
            settings.OffersManageViewInternalSelectionLevel = view.OffersManageViewInternalSelectionLevel;
            settings.OffersAcceptedAuthorisationManageViewLevel = view.OffersAcceptedAuthorisationManageViewLevel;
            settings.OffersRejectedAuthorisationManageViewLevel = view.OffersRejectedAuthorisationManageViewLevel;
            settings.OffersReturnedAuthorisationManageViewLevel = view.OffersReturnedAuthorisationManageViewLevel;
            settings.OrdersManageViewInternalSelectionLevel = view.OrdersManageViewInternalSelectionLevel;
            settings.OrdersDespatchedAuthorisationManageViewLevel = view.OrdersDespatchedAuthorisationManageViewLevel;
            settings.OrdersDeliveredAuthorisationManageViewLevel = view.OrdersDeliveredAuthorisationManageViewLevel;
            settings.OrdersReceivedAuthorisationManageViewLevel = view.OrdersReceivedAuthorisationManageViewLevel;
            settings.OrdersCollectedAuthorisationManageViewLevel = view.OrdersCollectedAuthorisationManageViewLevel;
            settings.OrdersClosedAuthorisationManageViewLevel = view.OrdersClosedAuthorisationManageViewLevel;
            settings.CampaignGeneralInfoExternalSelectionLevel = view.CampaignGeneralInfoExternalSelectionLevel;
            settings.RequiredListingGeneralInfoExternalSelectionLevel = view.RequiredListingGeneralInfoExternalSelectionLevel;
            settings.AvailableListingGeneralInfoExternalSelectionLevel = view.AvailableListingGeneralInfoExternalSelectionLevel;

            db.Entry(settings).State = EntityState.Modified;
            db.SaveChanges();

            return settings;
        }

        #endregion

        #region Delete

        public static void DeleteAppUserSettingsForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            DeleteAppUserSettingsForUser(db, appUserId);
            db.Dispose();
        }

        public static void DeleteAppUserSettingsForUser(ApplicationDbContext db, Guid appUserId)
        {
            AppUserSettings appUserSettings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);
            db.AppUserSettings.Remove(appUserSettings);
            db.SaveChanges();
        }

        #endregion
    }
}