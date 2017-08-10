using Distributor.Extenstions;
using Distributor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace Distributor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            ViewBag.UserName = User.Identity.GetFullName();
            
            ViewBag.BranchName = BranchHelpers.GetCurrentBranchForUser(AppUserHelpers.GetGuidFromUserGetAppUserId(User.Identity.GetAppUserId())).BranchName;

            return View();
        }
    }
}