using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Distributor.Helpers
{
    public static class CompanyAdminHelpers
    {
        #region Get

        public static CompanyAdminView GetCompanyAdminView(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return GetCompanyAdminView(db, user);
        }

        public static CompanyAdminView GetCompanyAdminView(ApplicationDbContext db, IPrincipal user)
        {
            //get company
            Company company = CompanyHelpers.GetCompanyForUser(user);

            //Get linked branches to this company
            List<Branch> branches = BranchHelpers.GetBranchesForCompany(db, company.CompanyId);

            //Build view
            CompanyAdminView companyAdminView = new CompanyAdminView()
            {
                CompanyDetails = company,
                RelatedBranches = branches
            };

            return companyAdminView;
        }

        #endregion

        #region Update

        public static bool UpdateCompanyFromCompanyAdminView(CompanyAdminView companyAdminView)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return UpdateCompanyFromCompanyAdminView(db, companyAdminView);
        }

        public static bool UpdateCompanyFromCompanyAdminView(ApplicationDbContext db, CompanyAdminView companyAdminView)
        {
            try
            {
                Company company = CompanyHelpers.UpdateCompany(db, 
                    companyAdminView.CompanyDetails.CompanyId, 
                    companyAdminView.CompanyDetails.HeadOfficeBranchId, 
                    companyAdminView.CompanyDetails.CompanyName, 
                    companyAdminView.CompanyDetails.CompanyRegistrationDetails, 
                    companyAdminView.CompanyDetails.CharityRegistrationDetails, 
                    companyAdminView.CompanyDetails.VATRegistrationDetails, 
                    companyAdminView.CompanyDetails.EntityStatus
                    );
                
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