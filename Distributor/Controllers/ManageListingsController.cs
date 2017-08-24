using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Distributor.Controllers
{
    [Authorize]
    public class ManageListingsController : Controller
    {
        public ActionResult Requirements()
        {
            return View();
        }
    }
}