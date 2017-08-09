using Distributor.Extenstions;
using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static AppUser GetAppUser(System.Security.Principal.IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetAppUser(db, user);
        }

        public static AppUser GetAppUser(ApplicationDbContext db, System.Security.Principal.IPrincipal user)
        {
            Guid appUserId;
            Guid.TryParse(user.Identity.GetAppUserId(), out appUserId);

            return db.AppUsers.Find(GetAppUser(db, appUserId));
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

        #endregion
    }
}