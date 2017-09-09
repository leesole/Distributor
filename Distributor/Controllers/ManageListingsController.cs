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
            List<RequirementListingManageView> model = RequirementListingManageHelpers.GetAllRequirementListingsManageView(User);
            return View(model);
        }

        public ActionResult Available()
        {
            List<AvailableListingManageView> model = AvailableListingManageHelpers.GetAllAvailableListingsManageView(User);
            return View(model);
        }

        public ActionResult Campaigns()
        {
            List<CampaignManageView> model = CampaignManageHelpers.GetAllCampaignsManageView(User);
            return View(model);
        }

        public ActionResult Offers()
        {
            List<OfferManageView> model = OfferManageHelpers.GetAllOffersManageView(User);
            return View(model);
        }

        public ActionResult Orders()
        {
            List<OrderManageView> model = OrderManageHelpers.GetAllOrdersManageView(User);
            return View(model);
        }
    }
}