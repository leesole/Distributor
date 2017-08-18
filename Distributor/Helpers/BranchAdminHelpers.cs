using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Distributor.Helpers
{
    public static class BranchAdminHelpers
    {
        #region Get

        public static List<BranchAdminView> GetBranchAdminViewList(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetBranchAdminViewList(db, user);
        }

        public static List<BranchAdminView> GetBranchAdminViewList(ApplicationDbContext db, IPrincipal user)
        {
            List<BranchAdminView> branchAdminViewList = new List<BranchAdminView>();

            //Get the AppUser of this User
            AppUser appUser = AppUserHelpers.GetAppUser(db, user);

            //Get the list of branches this User is linked to
            List<Branch> branchesForAppUser = (from bu in db.BranchUsers
                                               join b in db.Branches on bu.UserId equals appUser.AppUserId
                                               select b).ToList();

            foreach (Branch branch in branchesForAppUser)
            {
                //Build list of company users to add to this branchView
                List<BranchAdminViewCompanyUser> branchAdminCompanyUsers = new List<BranchAdminViewCompanyUser>();

                //Build a list of all users that are linked to this company
                List<AppUser> allUsersForCompany = (from bu in db.BranchUsers
                                                    join au in db.AppUsers on bu.UserId equals au.AppUserId
                                                    select au).ToList();

                //Build a list of all users for this branch
                List<AppUser> allUsersForThisBranch = (from bu in db.BranchUsers
                                                       join au in db.AppUsers on bu.UserId equals au.AppUserId
                                                       where bu.BranchId == branch.BranchId
                                                       select au).ToList();

                //Add all company users to the 'RelatedCompanyUsers' and set the flag to true if the user appears in the branch list
                List<BranchAdminViewCompanyUser> relatedCompanyUsers = new List<BranchAdminViewCompanyUser>();
                foreach (AppUser userForCompany in allUsersForCompany)
                {
                    //If the user appears in branchlist then set the 'linked' flag
                    bool found = false;
                    AppUser foundUser = allUsersForThisBranch.FirstOrDefault(x => x.AppUserId == userForCompany.AppUserId);
                    if (foundUser != null)
                        found = true;

                    BranchAdminViewCompanyUser branchAdminCompanyUser = new BranchAdminViewCompanyUser()
                    {
                        AppUserDetails = userForCompany,
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
                    EntityStatus = branch.EntityStatus,
                    CompanyUserListId = Guid.NewGuid(),  //used to identify the following list in the view (can't use BranchId as that is used for a different block in the table)
                    RelatedCompanyUsers = branchAdminCompanyUsers
                };

                branchAdminViewList.Add(branchAdminView);
            }
            
            return branchAdminViewList;
        }

        #endregion
    }
}