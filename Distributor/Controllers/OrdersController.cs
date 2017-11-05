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
            OrderEditView order = OrderEditHelpers.GetOrderEditView(db, id.Value, User);
            if (order == null)
            {
                return HttpNotFound();
            }

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

            order = OrderEditHelpers.GetOrderEditView(db, order.OrderId, User);

            ViewBag.ShowHistory = false;

            return View(order);
        }

        public ActionResult History()
        {
            List<OrderManageView> model = OrderManageHelpers.GetAllOrdersManageView(User, true);
            
            ViewBag.OutOrderCount = (from m in model
                                     where m.OrderOut == true
                                     select m).Count();

            ViewBag.InOrderCount = (from m in model
                                    where m.OrderOut == false
                                    select m).Count();

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
