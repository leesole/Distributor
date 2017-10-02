using Distributor.Helpers;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Distributor.Controllers
{
    [Authorize]
    public class GeneralInfoController : Controller
    {   
        public ActionResult Requirements()
        {
            List<RequirementListingGeneralInfoView> model = RequirementListingGeneralInfoHelpers.GetAllRequirementListingsGeneralInfoView(User);

            Company userCompany = CompanyHelpers.GetCompanyForUser(User);
            ViewBag.CurrentCompanyId = userCompany.CompanyId;

            return View(model);
        }

        public ActionResult Available()
        {
            List<AvailableListingGeneralInfoView> model = AvailableListingGeneralInfoHelpers.GetAllAvailableListingsGeneralInfoView(User);

            Company userCompany = CompanyHelpers.GetCompanyForUser(User);
            ViewBag.CurrentCompanyId = userCompany.CompanyId;

            return View(model);
        }

        public ActionResult Campaigns()
        {
            List<CampaignGeneralInfoView> model = CampaignGeneralInfoViewHelpers.GetAllCampaignsGeneralInfoView(User);

            //Do any filtering

            return View(model);
        }
    }
}