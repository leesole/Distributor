using Distributor.Helpers;
using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Distributor.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult GetBranchByCompany(Guid companyId)
        {
            return Json(ControlHelpers.AllBranchesForCompany(companyId));
        }

        //public ActionResult ActivateUser(Guid appUserId)
        //{
        //    return Json(new { success = true });
        //}

        //get branch object for branchid
        [HttpPost]
        public ActionResult GetBranchAddressDetailsForBranch(Guid branchId)
        {
            if (branchId != null)
            {
                Branch branchDetails = BranchHelpers.GetBranch(branchId);

                if (branchDetails != null)
                {
                    string branchBusinessType = EnumHelpers.GetDescription(branchDetails.BusinessType);

                    return Json(new { branchDetails, branchBusinessType, success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }
    }
}