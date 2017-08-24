using Distributor.Helpers;
using Distributor.ViewModels;
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
            List<RequirementListingManageView> model = RequirementListingManageHelpers.GetAllRequirementListingsManageViewForUserBranch(User);
            return View(model);
        }

        public ActionResult Available()
        {
            return View();
        }

        public ActionResult Campaigns()
        {
            List<CampaignManageView> model = CampaignGeneralManageHelpers.GetAllCampaignsManageViewForUserBranch(User);
            return View(model);
        }
    }
}