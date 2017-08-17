using Distributor.Extenstions;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Distributor.Helpers
{
    public static class UserAdminHelpers
    {
        #region Get

        public static List<UserAdminView> GetUserAdminViewListForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetUserAdminViewListForUser(db, user);
        }

        public static List<UserAdminView> GetUserAdminViewListForUser(ApplicationDbContext db, IPrincipal user)
        {
            List<UserAdminView> userAdminViewListForUser = new List<UserAdminView>();
            List<UserAdminRelatedBranchesView> relatedBranches = new List<UserAdminRelatedBranchesView>();

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            BranchUser branchUser = BranchUserHelpers.GetBranchUser(db, appUser.AppUserId, appUser.CurrentBranchId);

            switch (user.Identity.GetCurrentUserRole())
            {
                case "SuperUser":
                case "Admin": //Get all users for the company of this user
                    //List<AppUser> appUsersForCompany = (from b in db.BranchUsers
                    //                                    join a in db.AppUsers on b.UserId equals a.AppUserId
                    //                                    where b.CompanyId == branchUser.CompanyId
                    //                                    select a).ToList();

                    var branchUsersForCompany = (from b in db.BranchUsers
                                                 join a in db.AppUsers on b.UserId equals a.AppUserId
                                                 where b.CompanyId == branchUser.CompanyId
                                                 select new { AppUserId = b.UserId, BranchId = b.BranchId, BranchUserId = b.BranchUserId }).ToList();

                    foreach (var branchUserForCompany in branchUsersForCompany)
                    {
                        UserAdminRelatedBranchesView relatedBranch = new UserAdminRelatedBranchesView();
                        relatedBranch.AppUserId = branchUserForCompany.AppUserId;
                        relatedBranch.BranchId = branchUserForCompany.BranchId;
                        relatedBranch.BranchUserId = branchUserForCompany.BranchUserId;
                        relatedBranch.BranchUserDetails = BranchUserHelpers.GetBranchUser(db, branchUserForCompany.BranchUserId);
                        relatedBranch.BranchDetails = BranchHelpers.GetBranch(db, branchUserForCompany.BranchId);

                        relatedBranches.Add(relatedBranch);
                    }

                    List<AppUser> appUsersForCompany = (from b in branchUsersForCompany
                                                        join a in db.AppUsers on b.AppUserId equals a.AppUserId
                                                        select a).ToList();

                    foreach (AppUser appUserForCompany in appUsersForCompany)
                    {
                        UserAdminView userAdminView = new UserAdminView();
                        userAdminView.AppUserId = appUserForCompany.AppUserId;
                        userAdminView.FirstName = appUserForCompany.FirstName;
                        userAdminView.LastName = appUserForCompany.LastName;
                        userAdminView.AppUserEntityStatus = appUserForCompany.EntityStatus;
                        userAdminView.CurrentBranchId = appUserForCompany.CurrentBranchId;
                        userAdminView.LoginEmail = "";
                        userAdminView.RelatedBranches = (from rb in relatedBranches
                                                         where rb.AppUserId == appUserForCompany.AppUserId
                                                         select rb).ToList();

                        userAdminViewListForUser.Add(userAdminView);
                    }
                    break;

                case "Manager": //Get all users for the branch of this user
                    //List<AppUser> appUsersForBranch =  (from b in db.BranchUsers
                    //                                    join a in db.AppUsers on b.UserId equals a.AppUserId
                    //                                    where b.BranchId == appUser.CurrentBranchId
                    //                                    select a).ToList();

                    var branchUsersForBranch = (from b in db.BranchUsers
                                                join a in db.AppUsers on b.UserId equals a.AppUserId
                                                where b.BranchId == appUser.CurrentBranchId
                                                select new { AppUserId = b.UserId, BranchId = b.BranchId, BranchUserId = b.BranchUserId }).ToList();

                    foreach (var branchUserForBranch in branchUsersForBranch)
                    {
                        UserAdminRelatedBranchesView relatedBranch = new UserAdminRelatedBranchesView();
                        relatedBranch.AppUserId = branchUserForBranch.AppUserId;
                        relatedBranch.BranchId = branchUserForBranch.BranchId;
                        relatedBranch.BranchUserId = branchUserForBranch.BranchUserId;
                        relatedBranch.BranchUserDetails = BranchUserHelpers.GetBranchUser(db, branchUserForBranch.BranchUserId);
                        relatedBranch.BranchDetails = BranchHelpers.GetBranch(db, branchUserForBranch.BranchId);

                        relatedBranches.Add(relatedBranch);
                    }

                    List<AppUser> appUsersForBranch = (from b in branchUsersForBranch
                                                       join a in db.AppUsers on b.AppUserId equals a.AppUserId
                                                       select a).ToList();

                    foreach (AppUser appUserForBranch in appUsersForBranch)
                    {
                        UserAdminView userAdminView = new UserAdminView();
                        userAdminView.AppUserId = appUserForBranch.AppUserId;
                        userAdminView.FirstName = appUserForBranch.FirstName;
                        userAdminView.LastName = appUserForBranch.LastName;
                        userAdminView.AppUserEntityStatus = appUserForBranch.EntityStatus;
                        userAdminView.CurrentBranchId = appUserForBranch.CurrentBranchId;
                        userAdminView.LoginEmail = "";
                        userAdminView.RelatedBranches = (from rb in relatedBranches
                                                         where rb.AppUserId == appUserForBranch.AppUserId
                                                         select rb).ToList();

                        userAdminViewListForUser.Add(userAdminView);
                    }

                    break;
            }

            return userAdminViewListForUser;
        }

        #endregion
    }
}