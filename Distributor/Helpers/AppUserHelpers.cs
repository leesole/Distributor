using Distributor.Extenstions;
using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.EntityEnums;

namespace Distributor.Helpers
{
    public static class AppUserHelpers
    {
        #region Get

        public static AppUser GetAppUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetAppUser(db, appUserId);
        }

        public static AppUser GetAppUser(ApplicationDbContext db, Guid appUserId)
        {
            return db.AppUsers.Find(appUserId);
        }

        public static AppUser GetAppUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetAppUser(db, user);
        }

        public static AppUser GetAppUser(ApplicationDbContext db, IPrincipal user)
        {
            Guid appUserId;
            Guid.TryParse(user.Identity.GetAppUserId(), out appUserId);

            return GetAppUser(db, appUserId);
        }

        #endregion

        #region Create

        public static AppUser CreateAppUser(string firstName, string lastName, Guid currentBranchId, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return CreateAppUser(db, firstName, lastName, currentBranchId, entityStatus);
        }

        public static AppUser CreateAppUser(ApplicationDbContext db, string firstName, string lastName, Guid currentBranchId, EntityStatusEnum entityStatus)
        {
            AppUser appUser = new AppUser()
            {
                AppUserId = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                CurrentBranchId = currentBranchId,
                EntityStatus = entityStatus
            };
            db.AppUsers.Add(appUser);
            db.SaveChanges();

            return appUser;
        }

        #endregion

        #region Update

        public static AppUser UpdateCurrentBranchId(Guid appUserId, Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return UpdateCurrentBranchId(db, appUserId, branchId);
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
            return UpdateEntityStatus(db, appUserId, entityStatus);
        }

        public static AppUser UpdateEntityStatus(ApplicationDbContext db, Guid appUserId, EntityStatusEnum entityStatus)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            appUser.EntityStatus = entityStatus;
            db.Entry(appUser).State = EntityState.Modified;
            db.SaveChanges();

            return appUser;
        }

        public static AppUser UpdateAppUserExcludingCurrentBranchField(Guid appUserId, string firstName, string lastName, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return UpdateAppUserExcludingCurrentBranchField(db, appUserId, firstName, lastName, entityStatus);
        }

        public static AppUser UpdateAppUserExcludingCurrentBranchField(ApplicationDbContext db, Guid appUserId, string firstName, string lastName, EntityStatusEnum entityStatus)
        {
            AppUser appUser = GetAppUser(db, appUserId);
            appUser.FirstName = firstName;
            appUser.LastName = lastName;
            appUser.EntityStatus = entityStatus;
            db.Entry(appUser).State = EntityState.Modified;
            db.SaveChanges();

            return appUser;
        }

        #endregion

        #region Delete

        public static void DeleteAppUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            DeleteAppUser(db, appUserId);
        }

        public static void DeleteAppUser(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            db.AppUsers.Remove(appUser);
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

        #endregion
    }
}