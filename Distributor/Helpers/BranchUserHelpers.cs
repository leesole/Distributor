using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.Helpers
{
    public static class BranchUserHelpers
    {
        #region Get

        public static BranchUser GetBranchUser(Guid branchUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            BranchUser branchUser = GetBranchUser(db, branchUserId);
            db.Dispose();
            return branchUser;
        }

        public static BranchUser GetBranchUser(ApplicationDbContext db, Guid branchUserId)
        {
            return db.BranchUsers.Find(branchUserId);
        }

        public static BranchUser GetBranchUser(Guid appUserId, Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            BranchUser branchUser = GetBranchUser(db, appUserId, branchId);
            db.Dispose();
            return branchUser;
        }

        public static BranchUser GetBranchUser(ApplicationDbContext db, Guid appUserId, Guid branchId)
        {
            BranchUser branchUser = (from b in db.BranchUsers
                                     where (b.BranchId == branchId && b.UserId == appUserId)
                                     select b).FirstOrDefault();

            return branchUser;
        }

        public static BranchUser GetBranchUser(Guid appUserId, Guid branchId, Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            BranchUser branchUser = GetBranchUser(db, appUserId, branchId, companyId);
            db.Dispose();
            return branchUser;
        }

        public static BranchUser GetBranchUser(ApplicationDbContext db, Guid appUserId, Guid branchId, Guid companyId)
        {
            BranchUser branchUser = (from b in db.BranchUsers
                                     where (b.BranchId == branchId && b.UserId == appUserId && b.CompanyId == companyId)
                                     select b).FirstOrDefault();

            return branchUser;
        }

        //Returns the current branch/company/appuser for the user sent
        public static BranchUser GetBranchUserCurrentForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            BranchUser branchUser = GetBranchUserCurrentForUser(db, user);
            db.Dispose();
            return branchUser;
        }

        //Returns the current branch/company/appuser for the user sent
        public static BranchUser GetBranchUserCurrentForUser(ApplicationDbContext db, IPrincipal user)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            return GetBranchUser(db, appUser.AppUserId, appUser.CurrentBranchId);
        }

        public static List<BranchUser> GetBranchUsersForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<BranchUser> list = GetBranchUsersForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<BranchUser> GetBranchUsersForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<BranchUser> list = (from bu in db.BranchUsers
                                     where bu.UserId == appUserId
                                     select bu).Distinct().ToList();

            return list;
        }

        public static List<BranchUser> GetBranchUsersForUserWithRole(Guid appUserId, UserRoleEnum role)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<BranchUser> list = GetBranchUsersForUserWithRole(db, appUserId, role);
            db.Dispose();
            return list;
        }

        public static List<BranchUser> GetBranchUsersForUserWithRole(ApplicationDbContext db, Guid appUserId, UserRoleEnum role)
        {
            List<BranchUser> list = (from bu in db.BranchUsers
                                     where (bu.UserId == appUserId && bu.UserRole == role)
                                     select bu).Distinct().ToList();

            return list;
        }

        public static List<BranchUser> GetBranchUsersForBranch(Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<BranchUser> list = GetBranchUsersForBranch(db, branchId);
            db.Dispose();
            return list;
        }

        public static List<BranchUser> GetBranchUsersForBranch(ApplicationDbContext db, Guid branchId)
        {
            List<BranchUser> list = (from bu in db.BranchUsers
                                     where bu.BranchId == branchId
                                     select bu).Distinct().ToList();

            return list;
        }

        public static List<BranchUser> GetAdminBranchUsersForBranchExcludingUser(Guid branchId, Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<BranchUser> list = GetAdminBranchUsersForBranchExcludingUser(db, branchId, appUserId);
            db.Dispose();
            return list;
        }

        public static List<BranchUser> GetAdminBranchUsersForBranchExcludingUser(ApplicationDbContext db, Guid branchId, Guid appUserId)
        {
            List<BranchUser> list = (from bu in db.BranchUsers
                                     join au in db.AppUsers on bu.UserId equals au.AppUserId
                                     where (bu.BranchId == branchId && bu.UserRole == UserRoleEnum.Admin && bu.UserId != appUserId && au.EntityStatus == EntityStatusEnum.Active)
                                     select bu).Distinct().ToList();

            return list;
        }

        #endregion

        #region Create

        public static BranchUser CreateBranchUser(Guid userId, Guid branchId, Guid companyId, UserRoleEnum userRole, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            BranchUser branchUser = CreateBranchUser(db, userId, branchId, companyId, userRole, entityStatus);
            db.Dispose();
            return branchUser;
        }

        public static BranchUser CreateBranchUser(ApplicationDbContext db, Guid userId, Guid branchId, Guid companyId, UserRoleEnum userRole, EntityStatusEnum entityStatus)
        {
            BranchUser branchUser = new BranchUser()
            {
                BranchUserId = Guid.NewGuid(),
                UserId = userId,
                BranchId = branchId,
                CompanyId = companyId,
                UserRole = userRole,
                EntityStatus = entityStatus
            };
            db.BranchUsers.Add(branchUser);
            db.SaveChanges();

            return branchUser;
        }

        public static void CreateBranchAdminUsersForNewBranch(Branch branch, UserRoleEnum role)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            CreateBranchAdminUsersForNewBranch(db, branch, role);
            db.Dispose();
        }
        
        /// <summary>
        /// Adds all Admin users for the company of the new branch to the new branch
        /// </summary>
        /// <param name="db"></param>
        /// <param name="branchId">new branch id</param>
        public static void CreateBranchAdminUsersForNewBranch(ApplicationDbContext db, Branch branch, UserRoleEnum role)
        {
            List<AppUser> adminAppUsersForCompany = AppUserHelpers.GetAdminAppUsersForCompany(db, branch.CompanyId);

            foreach (AppUser adminUser in adminAppUsersForCompany)
            {
                BranchUser branchUser = BranchUserHelpers.GetBranchUser(db, adminUser.AppUserId, branch.BranchId, branch.CompanyId);

                //Only add if not already there
                if (branchUser == null)
                    BranchUserHelpers.CreateBranchUser(db, adminUser.AppUserId, branch.BranchId, branch.CompanyId, role, EntityStatusEnum.Active);
            }
        }

        public static void CreateBranchUserAdminRolesForUserForAllBranches(BranchUser branchUser, UserRoleEnum userRole)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            CreateBranchUserAdminRolesForUserForAllBranches(db, branchUser, userRole);
            db.Dispose();
        }

        public static void CreateBranchUserAdminRolesForUserForAllBranches(ApplicationDbContext db, BranchUser branchUser, UserRoleEnum userRole)
        {
            List<Branch> companyBranches = BranchHelpers.GetBranchesForCompany(db, branchUser.CompanyId);

            foreach (Branch branch in companyBranches)
            {
                BranchUser thisBranchUser = BranchUserHelpers.GetBranchUser(db, branchUser.UserId, branch.BranchId, branch.CompanyId);

                //Update if required else create new if missing
                if (thisBranchUser != null)
                {
                    //if this branchuser is having the status changed then just check any outstanding actions and remove
                    if (userRole != thisBranchUser.UserRole)
                    {
                        thisBranchUser.UserRole = userRole;
                        thisBranchUser.EntityStatus = EntityStatusEnum.Active;
                        db.Entry(branchUser).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                    BranchUserHelpers.CreateBranchUser(db, branchUser.UserId, branch.BranchId, branch.CompanyId, userRole, EntityStatusEnum.Active);
            }
        }

        #endregion

        #region Update

        public static BranchUser UpdateBranchUserStatus(Guid branchUserId, EntityStatusEnum newEntityStatus, Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            BranchUser updateBranchUser = UpdateBranchUserStatus(db, branchUserId, newEntityStatus, appUserId);
            db.Dispose();
            return updateBranchUser;
        }

        public static BranchUser UpdateBranchUserStatus(ApplicationDbContext db, Guid branchUserId, EntityStatusEnum newEntityStatus, Guid appUserId)
        {
            BranchUser branchUser = GetBranchUser(db, branchUserId);
            branchUser.PreviousEntityStatus = branchUser.EntityStatus;
            branchUser.EntityStatus = newEntityStatus;
            branchUser.EntityStatusChangeBy = appUserId;
            branchUser.EntityStatusChangeDate = DateTime.Now;

            db.Entry(branchUser).State = EntityState.Modified;
            db.SaveChanges();

            return branchUser;
        }

        public static BranchUser UpdateBranchUserStatus(BranchUser branchUser, EntityStatusEnum newEntityStatus, Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            BranchUser updateBranchUser = UpdateBranchUserStatus(db, branchUser, newEntityStatus, appUserId);
            db.Dispose();
            return updateBranchUser;
        }

        public static BranchUser UpdateBranchUserStatus(ApplicationDbContext db, BranchUser branchUser, EntityStatusEnum newEntityStatus, Guid appUserId)
        {
            branchUser.PreviousEntityStatus = branchUser.EntityStatus;
            branchUser.EntityStatus = newEntityStatus;
            branchUser.EntityStatusChangeBy = appUserId;
            branchUser.EntityStatusChangeDate = DateTime.Now;

            db.Entry(branchUser).State = EntityState.Modified;
            db.SaveChanges();

            return branchUser;
        }
        
        public static BranchUser UpdateBranchUserRole(Guid branchUserId, UserRoleEnum userRole)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            BranchUser updateBranchUser = UpdateBranchUserRole(db, branchUserId, userRole);
            db.Dispose();
            return updateBranchUser;
        }

        public static BranchUser UpdateBranchUserRole(ApplicationDbContext db, Guid branchUserId, UserRoleEnum userRole)
        {
            BranchUser branchUser = GetBranchUser(db, branchUserId);

            //if user role changing FROM or TO Super/Admin user then set appUser flags accordingly
            if (userRole == UserRoleEnum.SuperUser)
                AppUserHelpers.UpdateRoleFlags(db, branchUser.UserId, true, false);
            if (userRole == UserRoleEnum.Admin)
                AppUserHelpers.UpdateRoleFlags(db, branchUser.UserId, false, true);

            //if role changes to Admin/Super user from anything else then add all company branches to user and activate if required
            if ((userRole == UserRoleEnum.SuperUser || userRole == UserRoleEnum.Admin) && (branchUser.UserRole != UserRoleEnum.SuperUser && branchUser.UserRole != UserRoleEnum.Admin))
                BranchUserHelpers.CreateBranchUserAdminRolesForUserForAllBranches(db, branchUser, userRole);
            else
                branchUser.UserRole = userRole;

            branchUser.UserRole = userRole;
            db.Entry(branchUser).State = EntityState.Modified;
            db.SaveChanges();

            return branchUser;
        }

        public static void UpdateBranchUserRoleForAllBranches(Guid appUserId, UserRoleEnum userRole)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UpdateBranchUserRoleForAllBranches(db, appUserId, userRole);
            db.Dispose();
        }

        public static void UpdateBranchUserRoleForAllBranches(ApplicationDbContext db, Guid appUserId, UserRoleEnum userRole)
        {
            List<BranchUser> branchUserEntriesForUser = BranchUserHelpers.GetBranchUsersForUser(appUserId);

            foreach (BranchUser branchUser in branchUserEntriesForUser)
                UpdateBranchUserRole(db, branchUser.BranchUserId, userRole);
        }

        #endregion
    }
}