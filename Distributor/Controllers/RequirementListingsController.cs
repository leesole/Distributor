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
    public class RequirementListingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: RequirementListings
        //public ActionResult Index()
        //{
        //    return View(db.RequirementListings.ToList());
        //}

        //// GET: RequirementListings/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RequirementListing requirementListing = db.RequirementListings.Find(id);
        //    if (requirementListing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(requirementListing);
        //}

        // GET: RequirementListings/Create
        public ActionResult Create()
        {
            string[] callingUrlSegments = Request.UrlReferrer.Segments.Select(x => x.TrimEnd('/')).ToArray();
            string callingController = callingUrlSegments[callingUrlSegments.Count() - 2];
            string callingAction = callingUrlSegments[callingUrlSegments.Count() - 1];

            ViewBag.CallingController = callingController;

            RequirementListingAddView model = new RequirementListingAddView()
            {
                CallingAction = callingAction,
                CallingController = callingController
            };

            return View(model);
        }

        // POST: RequirementListings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemDescription,ItemType,QuantityRequired,QuantityFulfilled,QuantityOutstanding,UoM,RequiredFrom,RequiredTo,AcceptDamagedItems,DeliveryAvailable,ListingStatus,CampaignId,CallingAction,CallingController")] RequirementListingAddView requirementListing)
        {
            if (ModelState.IsValid)
            {
                RequirementListingHelpers.CreateRequirementListingFromRequirementListingAddView(requirementListing, User);
                //CampaignHelpers.CreateCampaignFromCampaignAddView(campaign, User);
                return RedirectToAction(requirementListing.CallingAction, requirementListing.CallingController);
            }

            return View(requirementListing);
        }

        //// GET: RequirementListings/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RequirementListing requirementListing = db.RequirementListings.Find(id);
        //    if (requirementListing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(requirementListing);
        //}

        //// POST: RequirementListings/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ListingId,ItemDescription,ItemType,QuantityRequired,QuantityFulfilled,QuantityOutstanding,UoM,RequiredFrom,RequiredTo,AcceptDamagedItems,DeliveryAvailable,ListingStatus,ListingBranchPostcode,ListingOriginatorAppUserId,ListingOriginatorBranchId,ListingOriginatorCompanyId,ListingOriginatorDateTime,CampaignId")] RequirementListing requirementListing)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(requirementListing).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(requirementListing);
        //}

        //// GET: RequirementListings/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RequirementListing requirementListing = db.RequirementListings.Find(id);
        //    if (requirementListing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(requirementListing);
        //}

        //// POST: RequirementListings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    RequirementListing requirementListing = db.RequirementListings.Find(id);
        //    db.RequirementListings.Remove(requirementListing);
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
