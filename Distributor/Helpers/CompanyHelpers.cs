using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Helpers
{
    public static class CompanyHelpers
    {
        #region Get

        public static List<Company> GetAllCompanies()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Company> list = GetAllCompanies(db);
            db.Dispose();
            return list;
        }

        public static List<Company> GetAllCompanies(ApplicationDbContext db)
        {
            return db.Companies.OrderBy(x => x.CompanyName).ToList(); 
        }

        public static List<Company> GetAllCompaniesForGroupForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Company> list = GetAllCompaniesForGroupForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Company> GetAllCompaniesForGroupForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<Company> allCompanies = GetAllCompanies(db);

            //Remove company for current user
            Company userCompany = CompanyHelpers.GetCompanyForUser(db, appUserId);
            allCompanies.Remove(userCompany);

            return allCompanies;
        }

        public static Company GetCompany(Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Company company = GetCompany(db, companyId);
            db.Dispose();
            return company;
        }

        public static Company GetCompany(ApplicationDbContext db, Guid companyId)
        {
            return db.Companies.Find(companyId);
        }

        public static Company GetCompanyForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Company company = GetCompanyForUser(db, user);
            db.Dispose();
            return company;
        }

        public static Company GetCompanyForUser(ApplicationDbContext db, IPrincipal user)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            return GetCompanyForUser(db, appUser);
        }

        public static Company GetCompanyForUser(AppUser appUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Company company = GetCompanyForUser(db, appUser);
            db.Dispose();
            return company;
        }

        public static Company GetCompanyForUser(ApplicationDbContext db, AppUser appUser)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUser(db, appUser.AppUserId, appUser.CurrentBranchId);
            Company company = GetCompany(db, branchUser.CompanyId);
            return company;
        }

        public static Company GetCompanyForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Company company = GetCompanyForUser(db, appUserId);
            db.Dispose();
            return company;
        }

        public static Company GetCompanyForUser(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            BranchUser branchUser = BranchUserHelpers.GetBranchUser(db, appUser.AppUserId, appUser.CurrentBranchId);
            Company company = GetCompany(db, branchUser.CompanyId);
            return company;
        }

        #endregion

        #region Create

        public static Company CreateCompany(Guid headOfficeBranchId, string companyName, string companyRegistrationDetails, string charityRegistrationDetails, string vatRegistrationDetails, bool allowBranchTrading, PrivacyLevelEnum privacyLevel, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Company company = CreateCompany(db, headOfficeBranchId, companyName, companyRegistrationDetails, charityRegistrationDetails, vatRegistrationDetails, allowBranchTrading, privacyLevel, entityStatus);
            db.Dispose();
            return company;
        }

        public static Company CreateCompany(ApplicationDbContext db, Guid headOfficeBranchId, string companyName, string companyRegistrationDetails, string charityRegistrationDetails, string vatRegistrationDetails, bool allowBranchTrading, PrivacyLevelEnum privacyLevel, EntityStatusEnum entityStatus)
        {
            Company company = new Company()
            {
                CompanyId = Guid.NewGuid(),
                HeadOfficeBranchId = headOfficeBranchId,
                CompanyName = companyName,
                CompanyRegistrationDetails = companyRegistrationDetails,
                CharityRegistrationDetails = charityRegistrationDetails,
                VATRegistrationDetails = vatRegistrationDetails,
                AllowBranchTrading = allowBranchTrading,
                PrivacyLevel = privacyLevel,
                EntityStatus = entityStatus
            };
            db.Companies.Add(company);
            db.SaveChanges();

            return company;
        }

        #endregion

        #region Update

        public static Company UpdateCompany(Guid companyId, Guid headOfficeBranchId, string companyName, string companyRegistrationDetails, string charityRegistrationDetails, string vatRegistrationDetails, bool allowBranchTrading, PrivacyLevelEnum privacyLevel, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Company company = UpdateCompany(db, companyId, headOfficeBranchId, companyName, companyRegistrationDetails, charityRegistrationDetails, vatRegistrationDetails, allowBranchTrading, privacyLevel, entityStatus);
            db.Dispose();
            return company;
        }

        public static Company UpdateCompany(ApplicationDbContext db, Guid companyId, Guid headOfficeBranchId, string companyName, string companyRegistrationDetails, string charityRegistrationDetails, string vatRegistrationDetails, bool allowBranchTrading, PrivacyLevelEnum privacyLevel, EntityStatusEnum entityStatus)
        {
            Company company = CompanyHelpers.GetCompany(companyId);
            company.HeadOfficeBranchId = headOfficeBranchId;
            company.CompanyName = companyName;
            company.CompanyRegistrationDetails = companyRegistrationDetails;
            company.CharityRegistrationDetails = charityRegistrationDetails;
            company.VATRegistrationDetails = vatRegistrationDetails;
            company.AllowBranchTrading = allowBranchTrading;
            company.PrivacyLevel = privacyLevel;
            company.EntityStatus = entityStatus;

            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();

            return company;
        }

        public static Company UpdateCompanyHeadOffice(Guid companyId, Guid headOfficeBranchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Company company = UpdateCompanyHeadOffice(db, companyId, headOfficeBranchId);
            db.Dispose();
            return company;
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