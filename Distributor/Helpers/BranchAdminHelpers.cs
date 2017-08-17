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
                    EntityStatus = branch.EntityStatus
                };

                branchAdminViewList.Add(branchAdminView);
            }

            return branchAdminViewList;
        }

        #endregion
    }
}