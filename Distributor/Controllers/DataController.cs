using Distributor.Helpers;
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
    }
}