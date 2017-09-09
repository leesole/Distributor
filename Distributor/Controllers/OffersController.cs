using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Distributor.Models;
using Distributor.Helpers;
using Distributor.ViewModels;

namespace Distributor.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Offers
        //public ActionResult Index()
        //{
        //    return View(db.Offers.ToList());
        //}

        // GET: Offers/Details
        public ActionResult Details(Guid? offerId)
        {
            if (offerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OfferManageView offerManageView = OfferManageHelpers.GetOfferManageViewForOffer(offerId.Value);

            if (offerManageView == null) //set to null if the value has changed from new to something else
            {
                return RedirectToAction("Offers", "ManageListings");
            }

            return View(offerManageView);
        }

        // GET: Offers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OfferId,ListingId,ListingType,OfferStatus,CurrentOfferQuantity,PreviousOfferQuantity,CounterOfferQuantity,OfferOriginatorAppUserId,OfferOriginatorBranchId,OfferOriginatorCompanyId,OfferOriginatorDateTime,OrderId,OrderOriginatorAppUserId,OrderOriginatorBranchId,OrderOriginatorCompanyId,OrderOriginatorDateTime")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                offer.OfferId = Guid.NewGuid();
                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(offer);
        }

        //// GET: Offers/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Offer offer = db.Offers.Find(id);
        //    if (offer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(offer);
        //}

        //// POST: Offers/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "OfferId,ListingId,ListingType,OfferStatus,CurrentOfferQuantity,PreviousOfferQuantity,CounterOfferQuantity,OfferOriginatorAppUserId,OfferOriginatorBranchId,OfferOriginatorCompanyId,OfferOriginatorDateTime,OrderId,OrderOriginatorAppUserId,OrderOriginatorBranchId,OrderOriginatorCompanyId,OrderOriginatorDateTime")] Offer offer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(offer).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(offer);
        //}

        //// GET: Offers/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Offer offer = db.Offers.Find(id);
        //    if (offer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(offer);
        //}

        //// POST: Offers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Offer offer = db.Offers.Find(id);
        //    db.Offers.Remove(offer);
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

        #region data manipulation

        public ActionResult AcceptOffer(string offerId)
        {
            if (offerId != null)
            {
                Offer offer = OfferHelpers.GetOffer(db, GeneralHelpers.GetGuidFromStringId(offerId));

                OfferHelpers.AcceptOffer(db, User, offer);

                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        public ActionResult RejectOffer(string offerId)
        {
            if (offerId != null)
            {
                Offer offer = OfferHelpers.GetOffer(db, GeneralHelpers.GetGuidFromStringId(offerId));

                OfferHelpers.RejectOffer(db, User, offer);

                return Json(new { success = true });
            }
            else
               return Json(new { success = false });
        }

        public ActionResult SubmitNewOffer(string offerId, string offerQuantity)
        {
            if (offerId != null && offerQuantity != null)
            {
                decimal offerQty = 0;
                decimal.TryParse(offerQuantity, out offerQty);

                if (offerQty > 0)
                {
                    Offer offer = OfferHelpers.GetOffer(db, GeneralHelpers.GetGuidFromStringId(offerId));

                    OfferHelpers.UpdateOffer(db, User, offer, offerQty);
                }

                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        public ActionResult SubmitCounterOffer(string offerId, string offerQuantity)
        {
            if (offerId != null && offerQuantity != null)
            {
                decimal offerQty = 0;
                decimal.TryParse(offerQuantity, out offerQty);

                if (offerQty > 0)
                {
                    Offer offer = OfferHelpers.GetOffer(db, GeneralHelpers.GetGuidFromStringId(offerId));

                    OfferHelpers.UpdateCounterOffer(db, User, offer, offerQty);
                }

                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        #endregion
    }
}
