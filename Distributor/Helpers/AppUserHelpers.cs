using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Distributor.Enums.UserEnums;

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

        #endregion

        #region Create

        public static AppUser CreateAppUser(string firstName, string lastName, UserRole userRole)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return CreateAppUser(db, firstName, lastName, userRole);
        }

        public static AppUser CreateAppUser(ApplicationDbContext db, string firstName, string lastName, UserRole userRole)
        {
            AppUser appUser = new AppUser()
            {
                AppUserId = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Role = userRole
            };
            db.AppUsers.Add(appUser);
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
    }
}