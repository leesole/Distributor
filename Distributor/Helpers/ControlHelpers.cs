using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Distributor.Enums.BranchEnums;

namespace Distributor.Helpers
{
    public static class ControlHelpers
    {
        #region DropDowns

        public static SelectList AllCompaniesListDropDown()
        {
            return new SelectList(CompanyHelpers.GetAllCompanies(), "CompanyId", "CompanyName");
        }

        public static SelectList AllCompaniesListDropDown(Guid companyId)
        {
            return new SelectList(CompanyHelpers.GetAllCompanies(), "CompanyId", "CompanyName", companyId);
        }

        public static SelectList AllBranchesForCompanyListDropDown(Guid companyId, Guid branchId)
        {
            return new SelectList(BranchHelpers.GetBranchesForCompany(companyId), "BranchId", "BranchName", branchId);
        }

        public static SelectList AllBranchesForCompany(Guid companyId)
        {
            return new SelectList(BranchHelpers.GetBranchesForCompany(companyId), "BranchId", "BranchName");
        }

        public static SelectList BusinessTypeEnumListDropDown()
        {
            var businessTypeEnumList = (from BusinessTypeEnum bt in Enum.GetValues(typeof(BusinessTypeEnum))
                                        select new
                                        {
                                            Id = bt,
                                            Name = EnumHelpers.GetDescription((BusinessTypeEnum)bt)
                                        });

            return new SelectList(businessTypeEnumList, "Id", "Name");
        }

        #endregion
    }
}