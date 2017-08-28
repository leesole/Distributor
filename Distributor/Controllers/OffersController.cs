﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Distributor.Models;

namespace Distributor.Controllers
{
    public class OffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Offers
        //public ActionResult Index()
        //{
        //    return View(db.Offers.ToList());
        //}

        //// GET: Offers/Details/5
        //public ActionResult Details(Guid? id)
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
    }
}
