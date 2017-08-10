using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Distributor.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Tasks()
        {
            return View();
        }

        public ActionResult UserAdmin()
        {
            return View();
        }

        public ActionResult BranchAdmin()
        {
            return View();
        }
        public ActionResult CompanyAdmin()
        {
            return View();
        }
    }
}