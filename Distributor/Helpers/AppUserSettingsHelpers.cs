using Distributor.Models;
using Distributor.Templates;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;

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
            int? requiredListingDashboardMaxDistance,
            double? requiredListingDashboardMaxAge,
            int? availableListingDashboardMaxDistance,
            double? availableListingDashboardMaxAge,
            ExternalSearchLevelEnum requiredListingDashboardtExternalSelectionLevel,
            ExternalSearchLevelEnum availableListingDashboardExternalSelectionLevel,
            int? requiredListingGeneralInfoMaxDistance,
            int? availableListingGeneralInfoMaxDistance,
            ExternalSearchLevelEnum requiredListingGeneralInfoExternalSelectionLevel,
            ExternalSearchLevelEnum availableListingGeneralInfoExternalSelectionLevel,
            InternalSearchLevelEnum requiredListingManageViewInternalSelectionLevel,
            InternalSearchLevelEnum availableListingManageViewInternalSelectionLevel)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserSettings appUserSettings = CreateAppUserSettings(db, appUserId, requiredListingDashboardMaxDistance, requiredListingDashboardMaxAge, availableListingDashboardMaxDistance, availableListingDashboardMaxAge, requiredListingDashboardtExternalSelectionLevel, availableListingDashboardExternalSelectionLevel, requiredListingGeneralInfoMaxDistance, availableListingGeneralInfoMaxDistance, requiredListingGeneralInfoExternalSelectionLevel, availableListingGeneralInfoExternalSelectionLevel, requiredListingManageViewInternalSelectionLevel, availableListingManageViewInternalSelectionLevel);
            db.Dispose();
            return appUserSettings;
        }

        public static AppUserSettings CreateAppUserSettings(ApplicationDbContext db, Guid appUserId,
            int? requiredListingDashboardMaxDistance,
            double? requiredListingDashboardMaxAge,
            int? availableListingDashboardMaxDistance,
            double? availableListingDashboardMaxAge,
            ExternalSearchLevelEnum requiredListingDashboardExternalSelectionLevel,
            ExternalSearchLevelEnum availableListingDashboardExternalSelectionLevel,
            int? requiredListingGeneralInfoMaxDistance,
            int? availableListingGeneralInfoMaxDistance,
            ExternalSearchLevelEnum requiredListingGeneralInfoExternalSelectionLevel,
            ExternalSearchLevelEnum availableListingGeneralInfoExternalSelectionLevel,
            InternalSearchLevelEnum requiredListingManageViewInternalSelectionLevel,
            InternalSearchLevelEnum availableListingManageViewInternalSelectionLevel)
        {
            AppUserSettings appUserSettings = new AppUserSettings()
            {
                AppUserSettingsId = Guid.NewGuid(),
                AppUserId = appUserId,
                RequiredListingDashboardMaxDistance = requiredListingDashboardMaxDistance,
                RequiredListingDashboardMaxAge = requiredListingDashboardMaxAge,
                AvailableListingDashboardMaxDistance = availableListingDashboardMaxDistance,
                AvailableListingDashboardMaxAge = availableListingDashboardMaxAge,
                RequiredListingDashboardExternalSelectionLevel = requiredListingDashboardExternalSelectionLevel,
                AvailableListingDashboardExternalSelectionLevel = availableListingDashboardExternalSelectionLevel,
                RequiredListingGeneralInfoMaxDistance = requiredListingGeneralInfoMaxDistance,
                AvailableListingGeneralInfoMaxDistance = availableListingGeneralInfoMaxDistance,
                RequiredListingManageViewInternalSelectionLevel = requiredListingManageViewInternalSelectionLevel,
                AvailableListingManageViewInternalSelectionLevel = availableListingManageViewInternalSelectionLevel,
                RequiredListingGeneralInfoExternalSelectionLevel = requiredListingGeneralInfoExternalSelectionLevel,
                AvailableListingGeneralInfoExternalSelectionLevel = availableListingGeneralInfoExternalSelectionLevel
            };
            db.AppUserSettings.Add(appUserSettings);
            db.SaveChanges();

            return appUserSettings;
        }

        public static AppUserSettings CreateAppUserSettingsForNewUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserSettings appUserSettings = CreateAppUserSettingsForNewUser(db, appUserId);
            db.Dispose();
            return appUserSettings;
        }

        public static AppUserSettings CreateAppUserSettingsForNewUser(ApplicationDbContext db, Guid appUserId)
        {
            AppUserSettingsTemplate template = new AppUserSettingsTemplate();

            AppUserSettings appUserSettings = new AppUserSettings()
            {
                AppUserSettingsId = Guid.NewGuid(),
                AppUserId = appUserId,
                RequiredListingDashboardMaxDistance = template.RequiredListingDashboardMaxDistance,
                RequiredListingDashboardMaxAge = template.RequiredListingDashboardMaxAge,
                AvailableListingDashboardMaxDistance = template.AvailableListingDashboardMaxDistance,
                AvailableListingDashboardMaxAge = template.AvailableListingDashboardMaxAge,
                RequiredListingDashboardExternalSelectionLevel = template.RequiredListingDashboardExternalSelectionLevel,
                AvailableListingDashboardExternalSelectionLevel = template.AvailableListingDashboardExternalSelectionLevel,
                RequiredListingGeneralInfoMaxDistance = template.RequiredListingGeneralInfoMaxDistance,
                AvailableListingGeneralInfoMaxDistance = template.AvailableListingGeneralInfoMaxDistance,
                RequiredListingManageViewInternalSelectionLevel = template.RequiredListingManageViewInternalSelectionLevel,
                AvailableListingManageViewInternalSelectionLevel = template.AvailableListingManageViewInternalSelectionLevel,
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
            
            settings.RequiredListingDashboardMaxDistance = view.RequiredListingDashboardMaxDistance;
            settings.RequiredListingDashboardMaxAge = view.RequiredListingDashboardMaxAge;
            settings.AvailableListingDashboardMaxDistance = view.AvailableListingDashboardMaxDistance;
            settings.AvailableListingDashboardMaxAge = view.AvailableListingDashboardMaxAge;
            settings.RequiredListingDashboardExternalSelectionLevel = view.RequiredListingDashboardExternalSelectionLevel;
            settings.AvailableListingDashboardExternalSelectionLevel = view.AvailableListingDashboardExternalSelectionLevel;
            settings.RequiredListingGeneralInfoMaxDistance = view.RequiredListingGeneralInfoMaxDistance;
            settings.AvailableListingGeneralInfoMaxDistance = view.AvailableListingGeneralInfoMaxDistance;
            settings.RequiredListingManageViewInternalSelectionLevel = view.RequiredListingManageViewInternalSelectionLevel;
            settings.AvailableListingManageViewInternalSelectionLevel = view.AvailableListingManageViewInternalSelectionLevel;
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