using Distributor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Distributor.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult GetBranchByCompany(Guid companyId)
        {
            return Json(ControlHelpers.AllBranchesForCompany(companyId));
        }
    }
}