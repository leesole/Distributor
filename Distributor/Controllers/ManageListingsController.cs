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

            ViewBag.RequiredCount = 0;
            ViewBag.AvailableCount = 0;

            try
            {
                ViewBag.RequiredCount = model.Count(x => x.OfferDetails.ListingType == ListingTypeEnum.Requirement);
                ViewBag.AvailableCount = model.Count(x => x.OfferDetails.ListingType == ListingTypeEnum.Available);
            }
            catch { }
            
            return View(model);
        }

        public ActionResult Orders()
        {
            List<OrderManageView> model = OrderManageHelpers.GetAllOrdersManageView(User);

            //If we allow branch trading then differentiate between branches for in/out trading, otherwise it is at company level
            Company company = CompanyHelpers.GetCompanyForUser(User);
            ViewBag.AllowBranchTrading = company.AllowBranchTrading;
            if (company.AllowBranchTrading)
            {
                Guid currentBranchId = AppUserHelpers.GetAppUser(User).CurrentBranchId;
                ViewBag.CurrentBranchOrCompanyId = currentBranchId;

                ViewBag.OutOrderCount = (from m in model
                                         where m.OrderDetails.OrderOriginatorBranchId.Value == currentBranchId
                                         select m).Count();

                ViewBag.InOrderCount = (from m in model
                                        where m.OrderDetails.OfferOriginatorBranchId.Value == currentBranchId
                                        select m).Count();
            }
            else
            {
                ViewBag.CurrentBranchOrCompanyId = company.CompanyId;

                ViewBag.OutOrderCount = (from m in model
                                         where m.OrderDetails.OrderOriginatorCompanyId.Value == company.CompanyId
                                         select m).Count();

                ViewBag.InOrderCount = (from m in model
                                        where m.OrderDetails.OfferOriginatorCompanyId.Value == company.CompanyId
                                        select m).Count();
            }

            //Set the authorisation levels and IDs for button activation on form
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(User);

            ViewBag.DespatchedAuthorisationLevel = settings.OrdersDespatchedAuthorisationManageViewLevel;
            ViewBag.DespatchedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersDespatchedAuthorisationManageViewLevel, User);
            ViewBag.DeliveredAuthorisationLevel = settings.OrdersDeliveredAuthorisationManageViewLevel;
            ViewBag.DeliveredAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersDeliveredAuthorisationManageViewLevel, User);
            ViewBag.ReceivedAuthorisationLevel = settings.OrdersReceivedAuthorisationManageViewLevel;
            ViewBag.ReceivedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersReceivedAuthorisationManageViewLevel, User);
            ViewBag.CollectedAuthorisationLevel = settings.OrdersCollectedAuthorisationManageViewLevel;
            ViewBag.CollectedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersCollectedAuthorisationManageViewLevel, User);
            ViewBag.ClosedAuthorisationLevel = settings.OrdersClosedAuthorisationManageViewLevel;
            ViewBag.ClosedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersClosedAuthorisationManageViewLevel, User);

            return View(model);
        }
    }
}