using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Distributor.Models;
using Distributor.ViewModels;
using Distributor.Helpers;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.OrderEnums;

namespace Distributor.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        

        // GET: Orders/Edit/5
        public ActionResult Details(Guid? id, bool showHistory)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderEditView order = OrderEditHelpers.GetOrderEditView(db, id.Value);
            if (order == null)
            {
                return HttpNotFound();
            }

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
            ViewBag.ReceivedAuthorisationLevel = settings.OrdersReceivedAuthorisationManageViewLevel;
            ViewBag.ReceivedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersReceivedAuthorisationManageViewLevel, User);
            ViewBag.CollectedAuthorisationLevel = settings.OrdersCollectedAuthorisationManageViewLevel;
            ViewBag.CollectedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersCollectedAuthorisationManageViewLevel, User);
            ViewBag.ClosedAuthorisationLevel = settings.OrdersClosedAuthorisationManageViewLevel;
            ViewBag.ClosedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersClosedAuthorisationManageViewLevel, User);

            ViewBag.ShowHistory = showHistory;

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(OrderEditView order)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form["despatchedbutton"] != null)
                {
                    //Update tables
                    OrderHelpers.ChangeOrderStatus(db, order.OrderId, OrderStatusEnum.Despatched, User);
                    return RedirectToAction("Orders", "ManageListings");
                }
                if (Request.Form["delivereddbutton"] != null)
                {
                    //Update tables
                    OrderHelpers.ChangeOrderStatus(db, order.OrderId, OrderStatusEnum.Delivered, User);
                    return RedirectToAction("Orders", "ManageListings");
                }
                if (Request.Form["collectedbutton"] != null)
                {
                    //Update tables
                    OrderHelpers.ChangeOrderStatus(db, order.OrderId, OrderStatusEnum.Collected, User);
                    return RedirectToAction("Orders", "ManageListings");
                }
                if (Request.Form["receivedbutton"] != null)
                {
                    //Update tables
                    OrderHelpers.ChangeOrderStatus(db, order.OrderId, OrderStatusEnum.Received, User);
                    return RedirectToAction("Orders", "ManageListings");
                }
                if (Request.Form["closedbutton"] != null)
                {
                    //Update tables
                    OrderHelpers.ChangeOrderStatus(db, order.OrderId, OrderStatusEnum.Closed, User);
                    return RedirectToAction("Orders", "ManageListings");
                }
            }

            //rebuild the missing details before returning to screen to show errors
            Order orderDetails = OrderHelpers.GetOrder(db, order.OrderId);

            if (orderDetails.OrderOriginatorAppUserId != null)
                if (orderDetails.OrderOriginatorAppUserId.Value != Guid.Empty)
                    order.OrderAppUser = AppUserHelpers.GetAppUser(db, orderDetails.OrderOriginatorAppUserId.Value);

            if (orderDetails.OrderOriginatorBranchId != null)
                if (orderDetails.OrderOriginatorBranchId.Value != Guid.Empty)
                    order.OrderBranchDetails = BranchHelpers.GetBranch(db, orderDetails.OrderOriginatorBranchId.Value);

            if (orderDetails.OfferOriginatorAppUserId != null)
                if (orderDetails.OfferOriginatorAppUserId.Value != Guid.Empty)
                    order.OfferAppUser = AppUserHelpers.GetAppUser(db, orderDetails.OfferOriginatorAppUserId.Value);

            if (orderDetails.OfferOriginatorBranchId != null)
                if (orderDetails.OfferOriginatorBranchId.Value != Guid.Empty)
                    order.OfferBranchDetails = BranchHelpers.GetBranch(db, orderDetails.OfferOriginatorBranchId.Value);

            if (orderDetails.ListingOriginatorAppUserId != null)
                if (orderDetails.ListingOriginatorAppUserId.Value != Guid.Empty)
                    order.ListingAppUser = AppUserHelpers.GetAppUser(db, orderDetails.ListingOriginatorAppUserId.Value);

            if (orderDetails.ListingOriginatorBranchId != null)
                if (orderDetails.ListingOriginatorBranchId.Value != Guid.Empty)
                    order.ListingBranchDetails = BranchHelpers.GetBranch(db, orderDetails.ListingOriginatorBranchId.Value);

            if (orderDetails.OfferId != null)
            {
                if (orderDetails.OfferId.Value != Guid.Empty)
                {
                    order.OfferDetails = OfferHelpers.GetOffer(db, orderDetails.OfferId.Value);
                    if (orderDetails.ListingId != null)
                    {
                        if (orderDetails.ListingId.Value != Guid.Empty)
                        {
                            switch (order.OfferDetails.ListingType)
                            {
                                case ListingTypeEnum.Available:
                                    order.AvailableListingDetails = AvailableListingHelpers.GetAvailableListing(db, orderDetails.ListingId.Value);
                                    break;
                                case ListingTypeEnum.Requirement:
                                    order.RequirementListingDetails = RequirementListingHelpers.GetRequirementListing(db, orderDetails.ListingId.Value);
                                    break;
                            }
                        }
                    }
                }
            }

            ViewBag.ShowHistory = false;

            return View(order);
        }

        public ActionResult History()
        {
            List<OrderManageView> model = OrderManageHelpers.GetAllOrdersManageView(User, true);

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
            ViewBag.ReceivedAuthorisationLevel = settings.OrdersReceivedAuthorisationManageViewLevel;
            ViewBag.ReceivedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersReceivedAuthorisationManageViewLevel, User);
            ViewBag.CollectedAuthorisationLevel = settings.OrdersCollectedAuthorisationManageViewLevel;
            ViewBag.CollectedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersCollectedAuthorisationManageViewLevel, User);
            ViewBag.ClosedAuthorisationLevel = settings.OrdersClosedAuthorisationManageViewLevel;
            ViewBag.ClosedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersClosedAuthorisationManageViewLevel, User);

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Data Manipulation

        public ActionResult ChangeOrderStatus(string orderId, string orderStatus)
        {
            if (orderId != null && orderStatus != null)
            {
                switch (orderStatus)
                {
                    case "Despatched":
                        OrderHelpers.ChangeOrderStatus(GeneralHelpers.GetGuidFromStringId(orderId), OrderStatusEnum.Despatched, User);
                        break;
                    case "Delivered":
                        OrderHelpers.ChangeOrderStatus(GeneralHelpers.GetGuidFromStringId(orderId), OrderStatusEnum.Delivered, User);
                        break;
                    case "Collected":
                        OrderHelpers.ChangeOrderStatus(GeneralHelpers.GetGuidFromStringId(orderId), OrderStatusEnum.Collected, User);
                        break;
                    case "Received":
                        OrderHelpers.ChangeOrderStatus(GeneralHelpers.GetGuidFromStringId(orderId), OrderStatusEnum.Received, User);
                        break;
                    case "Closed":
                        OrderHelpers.ChangeOrderStatus(GeneralHelpers.GetGuidFromStringId(orderId), OrderStatusEnum.Closed, User);
                        break;
                }

                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        #endregion
    }
}
