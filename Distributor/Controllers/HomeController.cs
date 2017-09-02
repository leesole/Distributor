using Distributor.Extenstions;
using Distributor.Helpers;
using Distributor.ViewModels;
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
            DashboardView dashboardView = new DashboardView();

            if (User.Identity.IsAuthenticated)
                dashboardView = DashboardHelpers.GetDashboardViewLogin(User);
            else
                dashboardView = DashboardHelpers.GetDashboardView();

            return View(dashboardView);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [Authorize]
        public ActionResult General()
        {
            return RedirectToAction("Requirements", "GeneralInfo");
        }

        [Authorize]
        public ActionResult Manage()
        {
            return RedirectToAction("Requirements", "ManageListings");
        }
    }
}