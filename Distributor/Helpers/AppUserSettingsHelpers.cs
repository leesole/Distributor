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

        public static AppUserSettings CreateAppUserSettings(Guid appUserId, int? globalMaxDistance, double? globalMaxAge, InternalSearchLevelEnum globalInternalSelectionLevel, ExternalSearchLevelEnum globalExternalSelectionLevel,
            int? availableListingGeneralInfoMaxDistance, int? availableListingRecentMaxDistance, double? availableListingRecentMaxAge, int? requiredListingGeneralInfoMaxDistance, int? requiredListingRecentMaxDistance,
            double? requiredListingRecentMaxAge, InternalSearchLevelEnum availableListingManageViewInternalSelectionLevel, InternalSearchLevelEnum requiredListingManageViewInternalSelectionLevel, ExternalSearchLevelEnum availableListingGeneralInfoExternalSelectionLevel,
            ExternalSearchLevelEnum requiredListingGeneralInfoExternalSelectionLevel, ExternalSearchLevelEnum availableListingRecentExternalSelectionLevel, ExternalSearchLevelEnum requiredListingRecentExternalSelectionLevel)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AppUserSettings appUserSettings = CreateAppUserSettings(db, appUserId, globalMaxDistance, globalMaxAge, globalInternalSelectionLevel, globalExternalSelectionLevel, availableListingGeneralInfoMaxDistance, availableListingRecentMaxDistance, availableListingRecentMaxAge, requiredListingGeneralInfoMaxDistance, requiredListingRecentMaxDistance, requiredListingRecentMaxAge, availableListingManageViewInternalSelectionLevel, requiredListingManageViewInternalSelectionLevel, availableListingGeneralInfoExternalSelectionLevel, requiredListingGeneralInfoExternalSelectionLevel, availableListingRecentExternalSelectionLevel, requiredListingRecentExternalSelectionLevel);
            db.Dispose();
            return appUserSettings;
        }

        public static AppUserSettings CreateAppUserSettings(ApplicationDbContext db, Guid appUserId, int? globalMaxDistance, double? globalMaxAge, InternalSearchLevelEnum globalInternalSelectionLevel, ExternalSearchLevelEnum globalExternalSelectionLevel,
            int? availableListingGeneralInfoMaxDistance, int? availableListingRecentMaxDistance, double? availableListingRecentMaxAge, int? requiredListingGeneralInfoMaxDistance, int? requiredListingRecentMaxDistance,
            double? requiredListingRecentMaxAge, InternalSearchLevelEnum availableListingManageViewInternalSelectionLevel, InternalSearchLevelEnum requiredListingManageViewInternalSelectionLevel, ExternalSearchLevelEnum availableListingGeneralInfoExternalSelectionLevel,
            ExternalSearchLevelEnum requiredListingGeneralInfoExternalSelectionLevel, ExternalSearchLevelEnum availableListingRecentExternalSelectionLevel, ExternalSearchLevelEnum requiredListingRecentExternalSelectionLevel)
        {
            AppUserSettings appUserSettings = new AppUserSettings()
            {
                AppUserSettingsId = Guid.NewGuid(),
                AppUserId = appUserId,
                GlobalMaxDistance = globalMaxDistance,
                GlobalMaxAge = globalMaxAge,
                GlobalInternalSelectionLevel = globalInternalSelectionLevel,
                GlobalExternalSelectionLevel = globalExternalSelectionLevel,
                AvailableListingGeneralInfoMaxDistance = availableListingGeneralInfoMaxDistance,
                AvailableListingRecentMaxDistance = availableListingRecentMaxDistance,
                AvailableListingRecentMaxAge = availableListingRecentMaxAge,
                RequiredListingGeneralInfoMaxDistance = requiredListingGeneralInfoMaxDistance,
                RequiredListingRecentMaxDistance = requiredListingRecentMaxDistance,
                RequiredListingRecentMaxAge = requiredListingRecentMaxAge,
                AvailableListingManageViewInternalSelectionLevel = availableListingManageViewInternalSelectionLevel,
                RequiredListingManageViewInternalSelectionLevel = requiredListingManageViewInternalSelectionLevel,
                AvailableListingGeneralInfoExternalSelectionLevel = availableListingGeneralInfoExternalSelectionLevel,
                RequiredListingGeneralInfoExternalSelectionLevel = requiredListingGeneralInfoExternalSelectionLevel,
                AvailableListingRecentExternalSelectionLevel = availableListingRecentExternalSelectionLevel,
                RequiredListingRecentExternalSelectionLevel = requiredListingRecentExternalSelectionLevel
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
                GlobalMaxDistance = template.GlobalMaxDistance,
                GlobalMaxAge = template.GlobalMaxAge,
                GlobalInternalSelectionLevel = template.GlobalInternalSelectionLevel,
                GlobalExternalSelectionLevel = template.GlobalExternalSelectionLevel,
                AvailableListingGeneralInfoMaxDistance = template.AvailableListingGeneralInfoMaxDistance,
                AvailableListingRecentMaxDistance = template.AvailableListingRecentMaxDistance,
                AvailableListingRecentMaxAge = template.AvailableListingRecentMaxAge,
                RequiredListingGeneralInfoMaxDistance = template.RequiredListingGeneralInfoMaxDistance,
                RequiredListingRecentMaxDistance = template.RequiredListingRecentMaxDistance,
                RequiredListingRecentMaxAge = template.RequiredListingRecentMaxAge,
                AvailableListingManageViewInternalSelectionLevel = template.AvailableListingManageViewInternalSelectionLevel,
                RequiredListingManageViewInternalSelectionLevel = template.RequiredListingManageViewInternalSelectionLevel,
                AvailableListingGeneralInfoExternalSelectionLevel = template.AvailableListingGeneralInfoExternalSelectionLevel,
                RequiredListingGeneralInfoExternalSelectionLevel = template.RequiredListingGeneralInfoExternalSelectionLevel,
                AvailableListingRecentExternalSelectionLevel = template.AvailableListingRecentExternalSelectionLevel,
                RequiredListingRecentExternalSelectionLevel = template.RequiredListingRecentExternalSelectionLevel
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

            settings.GlobalMaxDistance = view.GlobalMaxDistance;
            settings.GlobalMaxAge = view.GlobalMaxAge;
            settings.GlobalInternalSelectionLevel = view.GlobalInternalSelectionLevel;
            settings.GlobalExternalSelectionLevel = view.GlobalExternalSelectionLevel;
            settings.AvailableListingGeneralInfoMaxDistance = view.AvailableListingGeneralInfoMaxDistance;
            settings.AvailableListingRecentMaxDistance = view.AvailableListingRecentMaxDistance;
            settings.AvailableListingRecentMaxAge = view.AvailableListingRecentMaxAge;
            settings.RequiredListingGeneralInfoMaxDistance = view.RequiredListingGeneralInfoMaxDistance;
            settings.RequiredListingRecentMaxDistance = view.RequiredListingRecentMaxDistance;
            settings.RequiredListingRecentMaxAge = view.RequiredListingRecentMaxAge;
            settings.AvailableListingManageViewInternalSelectionLevel = view.AvailableListingManageViewInternalSelectionLevel;
            settings.RequiredListingManageViewInternalSelectionLevel = view.RequiredListingManageViewInternalSelectionLevel;
            settings.AvailableListingGeneralInfoExternalSelectionLevel = view.AvailableListingGeneralInfoExternalSelectionLevel;
            settings.RequiredListingGeneralInfoExternalSelectionLevel = view.RequiredListingGeneralInfoExternalSelectionLevel;
            settings.AvailableListingRecentExternalSelectionLevel = view.AvailableListingRecentExternalSelectionLevel;
            settings.RequiredListingRecentExternalSelectionLevel = view.RequiredListingRecentExternalSelectionLevel;

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