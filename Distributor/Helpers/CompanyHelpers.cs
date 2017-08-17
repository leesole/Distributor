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
    public static class CompanyHelpers
    {
        #region Get

        public static List<Company> GetAllCompanies()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetAllCompanies(db);
        }

        public static List<Company> GetAllCompanies(ApplicationDbContext db)
        {
            return db.Companies.OrderBy(x => x.CompanyName).ToList(); 
        }

        public static Company GetCompany(Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetCompany(db, companyId);
        }

        public static Company GetCompany(ApplicationDbContext db, Guid companyId)
        {
            return db.Companies.Find(companyId);
        }

        public static Company GetCompanyForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetCompanyForUser(db, user);
        }

        public static Company GetCompanyForUser(ApplicationDbContext db, IPrincipal user)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            return GetCompanyForUser(db, appUser);
        }

        public static Company GetCompanyForUser(AppUser appUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetCompanyForUser(db, appUser);
        }

        public static Company GetCompanyForUser(ApplicationDbContext db, AppUser appUser)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUser(db, appUser.AppUserId, appUser.CurrentBranchId);
            Company company = GetCompany(db, branchUser.CompanyId);
            return company;
        }

        #endregion

        #region Create

        public static Company CreateCompany(Guid headOfficeBranchId, string companyName, string companyRegistrationDetails, string charityRegistrationDetails, string vatRegistrationDetails, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return CreateCompany(db, headOfficeBranchId, companyName, companyRegistrationDetails, charityRegistrationDetails, vatRegistrationDetails, entityStatus);
        }

        public static Company CreateCompany(ApplicationDbContext db, Guid headOfficeBranchId, string companyName, string companyRegistrationDetails, string charityRegistrationDetails, string vatRegistrationDetails, EntityStatusEnum entityStatus)
        {
            Company company = new Company()
            {
                CompanyId = Guid.NewGuid(),
                HeadOfficeBranchId = headOfficeBranchId,
                CompanyName = companyName,
                CompanyRegistrationDetails = companyRegistrationDetails,
                CharityRegistrationDetails = charityRegistrationDetails,
                VATRegistrationDetails = vatRegistrationDetails,
                EntityStatus = entityStatus
            };
            db.Companies.Add(company);
            db.SaveChanges();

            return company;
        }

        #endregion

        #region Update

        public static Company UpdateCompanyHeadOffice(Guid companyId, Guid headOfficeBranchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return UpdateCompanyHeadOffice(db, companyId, headOfficeBranchId);
        }

        public static Company UpdateCompanyHeadOffice(ApplicationDbContext db, Guid companyId, Guid headOfficeBranchId)
        {
            Company company = GetCompany(db, companyId);

            company.HeadOfficeBranchId = headOfficeBranchId;
            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();

            return company;
        }

        #endregion
    }
}