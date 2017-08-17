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
        }
}