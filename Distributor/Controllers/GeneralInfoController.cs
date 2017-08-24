﻿using Distributor.Helpers;
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
            List<RequirementListingGeneralInfoView> model = RequirementListingGeneralInfoHelpers.GetAllRequirementListingsGeneralInfoView();

            //Do any filtering

            return View(model);
        }

        public ActionResult Available()
        {
            return View();
        }

        public ActionResult Campaigns()
        {
            List<CampaignGeneralInfoView> model = CampaignGeneralInfoViewHelpers.GetAllCampaignsGeneralInfoView();

            //Do any filtering

            return View(model);
        }
    }
}