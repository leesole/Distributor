using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserEnums;
using static Distributor.Enums.UserTaskEnums;

namespace Distributor.Helpers
{
    public static class BranchAdminHelpers
    {
        #region Get

        public static List<BranchAdminView> GetBranchAdminViewList(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<BranchAdminView> list = GetBranchAdminViewList(db, user);
            db.Dispose();
            return list;
        }

        public static List<BranchAdminView> GetBranchAdminViewList(ApplicationDbContext db, IPrincipal user)
        {
            List<BranchAdminView> branchAdminViewList = new List<BranchAdminView>();

            //Get the AppUser of this User
            AppUser appUser = AppUserHelpers.GetAppUser(db, user);

            //Get the list of branches this User is linked to
            List<Branch> branchesForAppUser = BranchHelpers.GetBranchesForUser(db, appUser.AppUserId);

            //Build a list of all users that are linked to this company
            List<AppUser> allUsersForCompany = AppUserHelpers.GetAppUsersForCompany(branchesForAppUser[0].CompanyId);

            foreach (Branch branch in branchesForAppUser)
            {
                //Build list of company users to add to this branchView
                List<BranchAdminViewCompanyUser> branchAdminCompanyUsers = new List<BranchAdminViewCompanyUser>();

                //Build a list of all users for this branch
                List<AppUser> allUsersForThisBranch = (from bu in db.BranchUsers
                                                       join au in db.AppUsers on bu.UserId equals au.AppUserId
                                                       where (bu.BranchId == branch.BranchId && bu.EntityStatus == EntityStatusEnum.Active)
                                                       select au).Distinct().ToList();

                //Add all company users to the 'RelatedCompanyUsers' and set the flag to true if the user appears in the branch list
                List<BranchAdminViewCompanyUser> relatedCompanyUsers = new List<BranchAdminViewCompanyUser>();
                foreach (AppUser userForCompany in allUsersForCompany)
                {
                    //If the user appears in branchlist then set the 'linked' flag
                    bool found = false;
                    AppUser foundUser = allUsersForThisBranch.FirstOrDefault(x => x.AppUserId == userForCompany.AppUserId);
                    if (foundUser != null)
                        found = true;

                    //get role from branchuser for this userForCompany/branch
                    BranchUser branchUserForCompany = BranchUserHelpers.GetBranchUser(db, userForCompany.AppUserId, branch.BranchId, branch.CompanyId);

                    BranchAdminViewCompanyUser branchAdminCompanyUser = new BranchAdminViewCompanyUser()
                    {
                        AppUserId = userForCompany.AppUserId,
                        CurrentBranchId = userForCompany.CurrentBranchId,
                        FirstName = userForCompany.FirstName,
                        LastName = userForCompany.LastName,
                        UserRole = branchUserForCompany.UserRole,
                        EntityStatus = userForCompany.EntityStatus,
                        LinkedToThisBranch = found
                    };

                    branchAdminCompanyUsers.Add(branchAdminCompanyUser);
                }


                //Add this list of users (as UserAdminRelatedBranchView(s)) to the model
                BranchAdminView branchAdminView = new BranchAdminView()
                {
                    BranchId = branch.BranchId,
                    CompanyId = branch.CompanyId,
                    BranchName = branch.BranchName,
                    BusinessType = branch.BusinessType,
                    AddressLine1 = branch.AddressLine1,
                    AddressLine2 = branch.AddressLine2,
                    AddressLine3 = branch.AddressLine3,
                    AddressTownCity = branch.AddressTownCity,
                    AddressCounty = branch.AddressCounty,
                    AddressPostcode = branch.AddressPostcode,
                    TelephoneNumber = branch.TelephoneNumber,
                    Email = branch.Email,
                    ContactName = branch.ContactName,
                    PrivacyLevel = branch.PrivacyLevel,
                    EntityStatus = branch.EntityStatus,
                    CompanyUserListId = Guid.NewGuid(),  //used to identify the following list in the view (can't use BranchId as that is used for a different block in the table)
                    RelatedCompanyUsers = branchAdminCompanyUsers
                };

                branchAdminViewList.Add(branchAdminView);
            }
            
            return branchAdminViewList;
        }

        #endregion

        #region Update

        public static bool UpdateBranchesFromBranchAdminView(List<BranchAdminView> branchesAdminView, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            bool updated = UpdateBranchesFromBranchAdminView(db, branchesAdminView, user);
            db.Dispose();
            return updated;
        }

        public static bool UpdateBranchesFromBranchAdminView(ApplicationDbContext db, List<BranchAdminView> branchesAdminView, IPrincipal user)
        {
            //Get logged in user details for Task creation (if required)
            AppUser loggedInUser = AppUserHelpers.GetAppUser(db, user);

            try
            {
                foreach (BranchAdminView branchAdminVeiw in branchesAdminView)
                {
                    //Get original branch record so that we can compare previous and current entity status'
                    Branch branch = BranchHelpers.GetBranch(db, branchAdminVeiw.BranchId);
                    EntityStatusEnum previousEntityStatus = branch.EntityStatus;

                    //Update branch
                    branch = BranchHelpers.UpdateBranch(db,
                        branchAdminVeiw.BranchId,
                        branchAdminVeiw.CompanyId,
                        branchAdminVeiw.BusinessType,
                        branchAdminVeiw.BranchName,
                        branchAdminVeiw.AddressLine1,
                        branchAdminVeiw.AddressLine2,
                        branchAdminVeiw.AddressLine3,
                        branchAdminVeiw.AddressTownCity,
                        branchAdminVeiw.AddressCounty,
                        branchAdminVeiw.AddressPostcode,
                        branchAdminVeiw.TelephoneNumber,
                        branchAdminVeiw.Email,
                        branchAdminVeiw.ContactName,
                        branchAdminVeiw.PrivacyLevel,
                        branchAdminVeiw.EntityStatus);

                    //if change of status from on-hold - anything then look for outstanding task and set to closed
                    if (branchAdminVeiw.EntityStatus != EntityStatusEnum.OnHold && previousEntityStatus == EntityStatusEnum.OnHold)
                    {
                        List<UserTask> activeTasksForThisBranch = UserTaskHelpers.GetUserTasksForBranch(db, branch.BranchId);

                        foreach (UserTask activeTaskForThisBranch in activeTasksForThisBranch)
                        {
                            UserTaskHelpers.UpdateEntityStatus(activeTaskForThisBranch.UserTaskId, EntityStatusEnum.Closed);
                        }
                    }

                    //If change of status to on-hold then create a Task
                    if (branchAdminVeiw.EntityStatus == EntityStatusEnum.OnHold && previousEntityStatus != EntityStatusEnum.OnHold)
                    {
                        UserTaskHelpers.CreateUserTask(TaskTypeEnum.BranchOnHold, "New branch on hold, awaiting administrator activation", branch.BranchId, loggedInUser.AppUserId, EntityStatusEnum.Active);
                    }

                    //Update Users link with Branch
                    foreach (BranchAdminViewCompanyUser companyUser in branchAdminVeiw.RelatedCompanyUsers)
                    {
                        //Try to find the user
                        BranchUser branchUser = BranchUserHelpers.GetBranchUser(db, companyUser.AppUserId, branchAdminVeiw.BranchId, branchAdminVeiw.CompanyId);

                        //Now check if user is checked in list then ensure it is on branchUser, else remove if it is on branchUser
                        if (companyUser.LinkedToThisBranch)
                        {
                            //if company user linked but not on BranchUser, add to BranchUser
                            if (branchUser == null)
                            { 
                                BranchUserHelpers.CreateBranchUser(db, companyUser.AppUserId, branchAdminVeiw.BranchId, branchAdminVeiw.CompanyId, UserRoleEnum.User, EntityStatusEnum.Active);
                            }
                            //if company user linked but not ACTIVE on BranchUser
                            else if (branchUser.EntityStatus != EntityStatusEnum.Active)
                            {
                                BranchUserHelpers.UpdateBranchUserStatus(db, branchUser, EntityStatusEnum.Active, companyUser.AppUserId);
                            }
                        }
                        else
                        {
                            //if company user not linked but is on BranchUser, remove from BranchUser by setting status to Inactive
                            if (branchUser != null)
                            {
                                BranchUserHelpers.UpdateBranchUserStatus(db, branchUser, EntityStatusEnum.Inactive, companyUser.AppUserId);
                            }
                        }
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