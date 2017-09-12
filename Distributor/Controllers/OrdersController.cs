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

        //// GET: Orders
        //public ActionResult Index()
        //{
        //    return View(db.Orders.ToList());
        //}

        //// GET: Orders/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// GET: Orders/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Orders/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "OrderId,OrderQuanity,OrderStatus,OrderCreationDateTime,OrderDistributionDateTime,OrderDeliveredDateTime,OrderCollectedDateTime,OrderClosedDateTime,OrderOriginatorAppUserId,OrderOriginatorBranchId,OrderOriginatorCompanyId,OrderOriginatorDateTime,OfferId,OfferOriginatorAppUserId,OfferOriginatorBranchId,OfferOriginatorCompanyId,ListingId,ListingOriginatorAppUserId,ListingOriginatorBranchId,ListingOriginatorCompanyId")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        order.OrderId = Guid.NewGuid();
        //        db.Orders.Add(order);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(order);
        //}

        // GET: Orders/Edit/5
        public ActionResult Details(Guid? id)
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
            ViewBag.CollectedAuthorisationLevel = settings.OrdersCollectedAuthorisationManageViewLevel;
            ViewBag.CollectedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersCollectedAuthorisationManageViewLevel, User);
            ViewBag.ClosedAuthorisationLevel = settings.OrdersClosedAuthorisationManageViewLevel;
            ViewBag.ClosedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OrdersClosedAuthorisationManageViewLevel, User);

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

            return View(order);
        }

        //// GET: Orders/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Order order = db.Orders.Find(id);
        //    db.Orders.Remove(order);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
