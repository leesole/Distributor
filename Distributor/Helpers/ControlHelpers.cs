using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Distributor.Helpers
{
    public static class ControlHelpers
    {
        #region DropDowns

        public static SelectList AllCompaniesListDropDown()
        {
            return new SelectList(CompanyHelpers.GetAllCompanies(), "CompanyId", "CompanyName");
        }

        public static SelectList AllBranchesForCompany(Guid companyId)
        {
            return new SelectList(BranchHelpers.GetBranchesForCompany(companyId), "BranchId", "BranchName");
        }

        #endregion
    }
}