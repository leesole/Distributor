using Distributor.Extenstions;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserTaskEnums;

namespace Distributor.Helpers
{
    public static class UserAdminHelpers
    {
        #region Get

        public static List<UserAdminView> GetUserAdminViewListForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<UserAdminView> list = GetUserAdminViewListForUser(db, user);
            db.Dispose();
            return list;
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
                    var branchUsersForCompany = (from b in db.BranchUsers
                                                 join a in db.AppUsers on b.UserId equals a.AppUserId
                                                 where b.CompanyId == branchUser.CompanyId
                                                 select new { AppUserId = b.UserId, BranchId = b.BranchId, BranchUserId = b.BranchUserId, CurrentBranchId = a.CurrentBranchId }).ToList();
                    var branchUsersForCompanyDistinct = branchUsersForCompany.Distinct();

                    foreach (var branchUserForCompany in branchUsersForCompanyDistinct)
                    {
                        UserAdminRelatedBranchesView relatedBranch = new UserAdminRelatedBranchesView();
                        relatedBranch.AppUserId = branchUserForCompany.AppUserId;
                        relatedBranch.BranchId = branchUserForCompany.BranchId;
                        relatedBranch.BranchUserId = branchUserForCompany.BranchUserId;

                        //relatedBranch.BranchUserDetails = BranchUserHelpers.GetBranchUser(db, branchUserForCompany.BranchUserId);
                        Branch branchDetails = BranchHelpers.GetBranch(db, branchUserForCompany.BranchId);

                        relatedBranch.BranchName = branchDetails.BranchName;
                        relatedBranch.AddressLine1 = branchDetails.AddressLine1;
                        relatedBranch.AddressTownCity = branchDetails.AddressTownCity;
                        relatedBranch.AddressPostcode = branchDetails.AddressPostcode;
                        
                        if (branchUserForCompany.BranchId == branchUserForCompany.CurrentBranchId)
                            relatedBranch.CurrentBranch = true;
                        else
                            relatedBranch.CurrentBranch = false;

                        relatedBranches.Add(relatedBranch);
                    }

                    List<AppUser> appUsersForCompany = (from b in branchUsersForCompany
                                                        join a in db.AppUsers on b.AppUserId equals a.AppUserId
                                                        select a).ToList();
                    var appUsersForCompanyDistinct = appUsersForCompany.Distinct();

                    foreach (AppUser appUserForCompany in appUsersForCompanyDistinct)
                    {
                        UserAdminView userAdminView = new UserAdminView();
                        userAdminView.AppUserId = appUserForCompany.AppUserId;
                        userAdminView.FirstName = appUserForCompany.FirstName;
                        userAdminView.LastName = appUserForCompany.LastName;
                        userAdminView.AppUserEntityStatus = appUserForCompany.EntityStatus;
                        userAdminView.CurrentBranchId = appUserForCompany.CurrentBranchId;
                        userAdminView.LoginEmail = appUserForCompany.LoginEmail;
                        userAdminView.RelatedBranches = (from rb in relatedBranches
                                                         where rb.AppUserId == appUserForCompany.AppUserId
                                                         select rb).ToList();

                        userAdminViewListForUser.Add(userAdminView);
                    }
                    break;

                case "Manager": //Get all users for the branches of this user (manager)
                    var branchList = (from bu in db.BranchUsers
                                      where bu.UserId == appUser.AppUserId
                                      select new { BranchId = bu.BranchId }).ToList();
                    var branchListDistinct = branchList.Distinct();

                    var branchUsersForBranch = (from b in db.BranchUsers
                                                join a in db.AppUsers on b.UserId equals a.AppUserId
                                                join c in branchListDistinct on b.BranchId equals c.BranchId
                                                where b.BranchId == appUser.CurrentBranchId
                                                select new { AppUserId = b.UserId, BranchId = b.BranchId, BranchUserId = b.BranchUserId, CurrentBranchId = a.CurrentBranchId }).ToList();
                    var branchUsersForBranchDistinct = branchUsersForBranch.Distinct();

                    foreach (var branchUserForBranch in branchUsersForBranchDistinct)
                    {
                        UserAdminRelatedBranchesView relatedBranch = new UserAdminRelatedBranchesView();
                        relatedBranch.AppUserId = branchUserForBranch.AppUserId;
                        relatedBranch.BranchId = branchUserForBranch.BranchId;
                        relatedBranch.BranchUserId = branchUserForBranch.BranchUserId;
                        //relatedBranch.BranchUserDetails = BranchUserHelpers.GetBranchUser(db, branchUserForBranch.BranchUserId);
                        Branch branchDetails = BranchHelpers.GetBranch(db, branchUserForBranch.BranchId);

                        relatedBranch.BranchName = branchDetails.BranchName;
                        relatedBranch.AddressLine1 = branchDetails.AddressLine1;
                        relatedBranch.AddressTownCity = branchDetails.AddressTownCity;
                        relatedBranch.AddressPostcode = branchDetails.AddressPostcode;

                        if (branchUserForBranch.BranchId == branchUserForBranch.CurrentBranchId)
                            relatedBranch.CurrentBranch = true;
                        else
                            relatedBranch.CurrentBranch = false;

                        relatedBranches.Add(relatedBranch);
                    }

                    List<AppUser> appUsersForBranch = (from b in branchUsersForBranch
                                                       join a in db.AppUsers on b.AppUserId equals a.AppUserId
                                                       select a).ToList();
                    var appUsersForBranchDistinct = appUsersForBranch.Distinct();

                    foreach (AppUser appUserForBranch in appUsersForBranchDistinct)
                    {
                        UserAdminView userAdminView = new UserAdminView();
                        userAdminView.AppUserId = appUserForBranch.AppUserId;
                        userAdminView.FirstName = appUserForBranch.FirstName;
                        userAdminView.LastName = appUserForBranch.LastName;
                        userAdminView.AppUserEntityStatus = appUserForBranch.EntityStatus;
                        userAdminView.CurrentBranchId = appUserForBranch.CurrentBranchId;
                        userAdminView.LoginEmail = appUserForBranch.LoginEmail;
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

        #region Update

        public static bool UpdateUsersFromUserAdminView(List<UserAdminView> userAdminViewForUser, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            bool updated = UpdateUsersFromUserAdminView(db, userAdminViewForUser, user);
            db.Dispose();
            return updated;
        }

        public static bool UpdateUsersFromUserAdminView(ApplicationDbContext db, List<UserAdminView> userAdminViewForUser, IPrincipal user)
        {
            //Get logged in user details for Task creation (if required)
            AppUser loggedInUser = AppUserHelpers.GetAppUser(db, user);

            try
            {
                foreach (UserAdminView userAdminView in userAdminViewForUser)
                {
                    //Get original appUser record so that we can compare previous and current entity status'
                    AppUser appUser = AppUserHelpers.GetAppUser(db, userAdminView.AppUserId);
                    EntityStatusEnum previousEntityStatus = appUser.EntityStatus;

                    //Update the AppUser record (except Current Branch as that was done in real time)
                    appUser = AppUserHelpers.UpdateAppUserExcludingCurrentBranchField(db,
                        userAdminView.AppUserId,
                        userAdminView.FirstName,
                        userAdminView.LastName,
                        userAdminView.AppUserEntityStatus);

                    //if change of status from on-hold - anything then look for outstanding task and set to closed
                    if (userAdminView.AppUserEntityStatus != EntityStatusEnum.OnHold && previousEntityStatus == EntityStatusEnum.OnHold)
                    {
                        List<UserTask> activeTasksForThisUser = UserTaskHelpers.GetUserTasksForUser(db, appUser.AppUserId);

                        foreach(UserTask activeTaskForThisUser in activeTasksForThisUser)
                        {
                            UserTaskHelpers.UpdateEntityStatus(activeTaskForThisUser.UserTaskId, EntityStatusEnum.Closed);
                        }
                    }

                    //If change of status to on-hold then create a Task
                    if (userAdminView.AppUserEntityStatus == EntityStatusEnum.OnHold && previousEntityStatus != EntityStatusEnum.OnHold)
                    {
                        UserTaskHelpers.CreateUserTask(TaskTypeEnum.UserOnHold, "New user on hold, awaiting administrator/manager activation", appUser.AppUserId, loggedInUser.AppUserId, EntityStatusEnum.Active);
                    }

                    //Update the User Role on each Branch
                    foreach (UserAdminRelatedBranchesView relatedBranch in userAdminView.RelatedBranches)
                    {
                        BranchUserHelpers.UpdateBranchUserRole(db, relatedBranch.BranchUserId, relatedBranch.UserRole);
                    }
                }
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return false;
            }
        }

        #endregion
        }
}