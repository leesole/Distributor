using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Distributor.Enums.EntityEnums;

namespace Distributor.Helpers
{
    public static class CompanyHelpers
    {
        #region Get

        public static Company GetCompany(Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetCompany(db, companyId);
        }

        public static Company GetCompany(ApplicationDbContext db, Guid companyId)
        {
            return db.Companies.Find(companyId);
        }

        #endregion

        #region Create

        public static Company CreateCompany(Guid headOfficeBranchId, string companyName, string companyRegistrationDetails, string charityRegistrationDetails, string vatRegistrationDetails)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return CreateCompany(db, headOfficeBranchId, companyName, companyRegistrationDetails, charityRegistrationDetails, vatRegistrationDetails);
        }

        public static Company CreateCompany(ApplicationDbContext db, Guid headOfficeBranchId, string companyName, string companyRegistrationDetails, string charityRegistrationDetails, string vatRegistrationDetails)
        {
            Company company = new Company()
            {
                ComapnyId = Guid.NewGuid(),
                HeadOfficeBranchId = headOfficeBranchId,
                CompanyName = companyName,
                CompanyRegistrationDetails = companyRegistrationDetails,
                CharityRegistrationDetails = charityRegistrationDetails,
                VATRegistrationDetails = vatRegistrationDetails,
                EntityStatus = EntityStatus.Active
            };
            db.Companies.Add(company);
            db.SaveChanges();

            return company;
        }

        #endregion
    }
}