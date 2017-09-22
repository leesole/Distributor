﻿using Distributor.Extenstions;
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

            //If we allow branch trading then differentiate between branches for in/out trading, otherwise it is at company level
            Company company = CompanyHelpers.GetCompanyForUser(User);
            ViewBag.AllowBranchTrading = company.AllowBranchTrading;
            if (company.AllowBranchTrading)
                ViewBag.CurrentBranchOrCompanyId = AppUserHelpers.GetAppUser(User).CurrentBranchId;
            else
                ViewBag.CurrentBranchOrCompanyId = company.CompanyId;

            //Set the authorisation levels and IDs for button activation on form
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(User);

            ViewBag.AcceptedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OffersAcceptedAuthorisationManageViewLevel, User);
            ViewBag.RejectedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OffersRejectedAuthorisationManageViewLevel, User);
            ViewBag.ReturnedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OffersReturnedAuthorisationManageViewLevel, User);

            return View(model);
        }

        public ActionResult Orders()
        {
            List<OrderManageView> model = OrderManageHelpers.GetAllOrdersManageView(User);

            //If we allow branch trading then differentiate between branches for in/out trading, otherwise it is at company level
            Company company = CompanyHelpers.GetCompanyForUser(User);
            ViewBag.AllowBranchTrading = company.AllowBranchTrading;
            if (company.AllowBranchTrading)
                ViewBag.CurrentBranchOrCompanyId = AppUserHelpers.GetAppUser(User).CurrentBranchId;
            else
                ViewBag.CurrentBranchOrCompanyId = company.CompanyId;

            //Set the authorisation levels and IDs for button activation on form
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(User);

            ViewBag.DespatchedAuthorisationLevel = settings.OrdersDespatchedAuthorisationManageViewLevel;
            ViewBag.DespatchedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersDespatchedAuthorisationManageViewLevel, User);
            ViewBag.DeliveredAuthorisationLevel = settings.OrdersDeliveredAuthorisationManageViewLevel;
            ViewBag.DeliveredAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersDeliveredAuthorisationManageViewLevel, User);
            ViewBag.CollectedAuthorisationLevel = settings.OrdersCollectedAuthorisationManageViewLevel;
            ViewBag.CollectedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersCollectedAuthorisationManageViewLevel, User);
            ViewBag.ClosedAuthorisationLevel = settings.OrdersClosedAuthorisationManageViewLevel;
            ViewBag.ClosedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersClosedAuthorisationManageViewLevel, User);
            
            return View(model);
        }
    }
}