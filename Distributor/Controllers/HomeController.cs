using Distributor.Extenstions;
using Distributor.Helpers;
using Distributor.Models;
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
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard", "Home");

            return View();
        }

        public ActionResult Dashboard()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            DashboardView dashboardView = new DashboardView();

            ViewBag.OutstandingTasks = 0;
            ViewBag.OutstandingActions = 0;
            ViewBag.OutstandingOffers = 0;
            ViewBag.Orders = 0;
            ViewBag.CurrentRequests = 0;
            ViewBag.CurrentAvailable = 0;
            ViewBag.RequestsOutstanding = 0;
            ViewBag.RequestsFulfilled = 0;
            ViewBag.RequestsTotal = 0;

            if (User.Identity.IsAuthenticated)
            {
                dashboardView = DashboardHelpers.GetDashboardViewLogin(User);
                ViewBag.OutstandingActions = UserActionHelpers.GetActionsForUser(User).Count();
                if (dashboardView.UserTaskList != null)
                    ViewBag.OustandingTasks = dashboardView.UserTaskList.Count();
                if (dashboardView.RequirementListingList != null)
                {
                    ViewBag.CurrentRequests = dashboardView.RequirementListingList.Count();
                    ViewBag.RequestsOutstanding = DashboardHelpers.GetRequirementsOutstandingFromDashboardView(dashboardView);
                    ViewBag.RequestsFulfilled = DashboardHelpers.GetRequirementsFulfilledFromDashboardView(dashboardView);
                    ViewBag.RequestsTotal = DashboardHelpers.GetRequirementsTotalFromDashboardView(dashboardView);
                }
                if (dashboardView.AvailableListingList != null)
                {
                    ViewBag.CurrentAvailable = dashboardView.AvailableListingList.Count();
                    ViewBag.AvailableOutstanding = DashboardHelpers.GetAvailableOutstandingFromDashboardView(dashboardView);
                    ViewBag.AvailableFulfilled = DashboardHelpers.GetAvailableFulfilledFromDashboardView(dashboardView);
                    ViewBag.AvailableTotal = DashboardHelpers.GetAvailableTotalFromDashboardView(dashboardView);
                }
                if (dashboardView.OfferList != null)
                    ViewBag.OutstandingOffers = dashboardView.OfferList.Count();
                if (dashboardView.OrderList != null)
                    ViewBag.Orders = dashboardView.OrderList.Count();
            }
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

        public ActionResult Campaigns()
        {
            List<Campaign> model = CampaignHelpers.GetAllCampaigns();
            return View(model);
        }

        public ActionResult Requests()
        {
            List<RequirementListing> model = RequirementListingHelpers.GetAllRequirementListings();
            return View(model);
        }
    }
}