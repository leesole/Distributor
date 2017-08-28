using Distributor.Extenstions;
using Distributor.Helpers;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Distributor.ViewModels.OfferViews;

namespace Distributor.Controllers
{
    [Authorize]
    public class ManageListingsController : Controller
    {
        public ActionResult Requirements()
        {
            List<RequirementListingManageView> model = RequirementListingManageHelpers.GetAllRequirementListingsManageViewForUser(User);
            return View(model);
        }

        public ActionResult Available()
        {
            List<AvailableListingManageView> model = AvailableListingManageHelpers.GetAllAvailableListingsManageViewForUser(User);
            return View(model);
        }

        public ActionResult Campaigns()
        {
            List<CampaignManageView> model = CampaignManageHelpers.GetAllCampaignsManageViewForUser(User);
            return View(model);
        }

        public ActionResult Offers()
        {
            List<OfferManageView> model = OfferManageHelpers.GetAllOffersManageViewForUser(User);
            return View(model);
        }

        public ActionResult Orders()
        {
            List<OrderManageView> model = OrderManageHelpers.GetAllOrdersManageViewForUserBranch(User);
            return View(model);
        }
    }
}