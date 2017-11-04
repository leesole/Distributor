using Distributor.Extenstions;
using Distributor.Helpers;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Distributor.Enums.GeneralEnums;
using static Distributor.ViewModels.OfferViews;

namespace Distributor.Controllers
{
    /// <summary>
    /// NOTE _ ANY CHANGES TO ANY OF THESE MODELS NEED TO BE REFELECTED IN THE 'HISTORY' OF THE CONTROLLER FOR THE CHANGE,
    /// 
    /// i.e. If you change the 'Requirements' controller below, then change the controller for RequirementsListing/History
    /// </summary>
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

            ViewBag.RequiredCount = 0;
            ViewBag.AvailableCount = 0;

            try
            {
                ViewBag.RequiredCount = model.Count(x => x.OfferDetails.ListingType == ListingTypeEnum.Requirement);
            } catch { }
            try
            { 
                ViewBag.AvailableCount = model.Count(x => x.OfferDetails.ListingType == ListingTypeEnum.Available);
            } catch { }
            
            return View(model);
        }

        public ActionResult Orders()
        {
            List<OrderManageView> model = OrderManageHelpers.GetAllOrdersManageView(User);

            ViewBag.OutOrderCount = (from m in model
                                     where m.OrderOut == true
                                     select m).Count();

            ViewBag.InOrderCount = (from m in model
                                    where m.OrderOut == false
                                    select m).Count();

            return View(model);
        }
    }
}