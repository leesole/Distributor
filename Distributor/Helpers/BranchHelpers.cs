using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.BranchEnums;
using static Distributor.Enums.EntityEnums;

namespace Distributor.Helpers
{
    public static class BranchHelpers
    {
        #region Get

        public static Branch GetBranch(Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetBranch(db, branchId);
        }

        public static Branch GetBranch(ApplicationDbContext db, Guid branchId)
        {
            return db.Branches.Find(branchId);
        }

        public static List<Branch> GetBranchesForCompany(Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetBranchesForCompany(db, companyId);
        }

        public static List<Branch> GetBranchesForCompany(ApplicationDbContext db, Guid companyId)
        {
            List<Branch> branchesForCompany = (from b in db.Branches
                                               where b.CompanyId == companyId
                                               orderby b.EntityStatus, b.BranchName
                                               select b).ToList();

            return branchesForCompany;
        }

        public static Branch GetCurrentBranchForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetCurrentBranchForUser(db, appUserId);
        }

        public static Branch GetCurrentBranchForUser(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = (from b in db.Branches
                             where b.BranchId == appUser.CurrentBranchId
                             select b).FirstOrDefault();

            return branch;
        }

        #endregion

        #region Create

        public static Branch CreateBranch(Branch branch)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return CreateBranch(db, branch);
        }

        public static Branch CreateBranch(ApplicationDbContext db, Branch branch)
        {
            branch.BranchId = Guid.NewGuid();
            db.Branches.Add(branch);
            db.SaveChanges();

            return branch;

        }

        public static Branch CreateBranch(Guid companyId, BusinessTypeEnum businessType, string branchName, string addressLine1, string addressLine2, string addressLine3, string addressTownCity, string addressCounty, string addressPostcode, string telephoneNumber, string email, string contactName, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return CreateBranch(db, companyId, businessType, branchName, addressLine1, addressLine2, addressLine3, addressTownCity, addressCounty, addressPostcode, telephoneNumber, email, contactName, entityStatus);
        }

        public static Branch CreateBranch(ApplicationDbContext db, Guid companyId, BusinessTypeEnum businessType, string branchName, string addressLine1, string addressLine2, string addressLine3, string addressTownCity, string addressCounty, string addressPostcode, string telephoneNumber, string email, string contactName, EntityStatusEnum entityStatus)
        {
            Branch branch = new Branch()
            {
                BranchId = Guid.NewGuid(),
                CompanyId = companyId,
                BusinessType = businessType,
                BranchName = branchName,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                AddressLine3 = addressLine3,
                AddressTownCity = addressTownCity,
                AddressCounty = addressCounty,
                AddressPostcode = addressPostcode,
                TelephoneNumber = telephoneNumber,
                Email = email,
                ContactName = contactName,
                EntityStatus = entityStatus
            };
            db.Branches.Add(branch);
            db.SaveChanges();

            return branch;
        }

        #endregion

        #region Update

        public static Branch UpdateBranch(Guid branchId, Guid companyId, BusinessTypeEnum businessType, string branchName, string addressLine1, string addressLine2, string addressLine3, string addressTownCity, string addressCounty, string addressPostcode, string telephoneNumber, string email, string contactName, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return UpdateBranch(db, branchId, companyId, businessType, branchName, addressLine1, addressLine2, addressLine3, addressTownCity, addressCounty, addressPostcode, telephoneNumber, email, contactName, entityStatus);
        }

        public static Branch UpdateBranch(ApplicationDbContext db, Guid branchId, Guid companyId, BusinessTypeEnum businessType, string branchName, string addressLine1, string addressLine2, string addressLine3, string addressTownCity, string addressCounty, string addressPostcode, string telephoneNumber, string email, string contactName, EntityStatusEnum entityStatus)
        {
            Branch branch = GetBranch(db, branchId);
            branch.CompanyId = companyId;
            branch.BranchName = branchName;
            branch.BusinessType = businessType;
            branch.AddressLine1 = addressLine1;
            branch.AddressLine2 = addressLine2;
            branch.AddressLine3 = addressLine3;
            branch.AddressTownCity = addressTownCity;
            branch.AddressCounty = addressCounty;
            branch.AddressPostcode = addressPostcode;
            branch.TelephoneNumber = telephoneNumber;
            branch.Email = email;
            branch.ContactName = contactName;
            branch.EntityStatus = entityStatus;
            
            db.Entry(branch).State = EntityState.Modified;
            db.SaveChanges();

            return branch;
        }

        public static Branch UpdateEntityStatus(Guid branchId, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return UpdateEntityStatus(db, branchId, entityStatus);
        }

        public static Branch UpdateEntityStatus(ApplicationDbContext db, Guid branchId, EntityStatusEnum entityStatus)
        {
            Branch branch = BranchHelpers.GetBranch(db, branchId);
            branch.EntityStatus = entityStatus;
            db.Entry(branch).State = EntityState.Modified;
            db.SaveChanges();

            return branch;
        }

        #endregion
    }
}