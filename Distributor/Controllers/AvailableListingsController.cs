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

namespace Distributor.Controllers
{
    public class AvailableListingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: AvailableListings
        //public ActionResult Index()
        //{
        //    return View(db.AvailableListings.ToList());
        //}

        // GET: AvailableListings/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableListingEditView availableListing = AvailableListingEditHelpers.GetAvailableListingEditView(db, id.Value);
            if (availableListing == null)
            {
                return HttpNotFound();
            }
            return View(availableListing);
        }

        // GET: AvailableListings/Create
        public ActionResult Create()
        {
            string callingController = "Home";
            string callingAction = "Index";

            try
            {
                string[] callingUrlSegments = Request.UrlReferrer.Segments.Select(x => x.TrimEnd('/')).ToArray();
                callingController = callingUrlSegments[callingUrlSegments.Count() - 2];
                callingAction = callingUrlSegments[callingUrlSegments.Count() - 1];
            }
            catch { }

            ViewBag.CallingController = callingController;

            AvailableListingAddView model = new AvailableListingAddView()
            {
                CallingAction = callingAction,
                CallingController = callingController
            };

            return View(model);
        }

        // POST: AvailableListings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemDescription,ItemType,QuantityRequired,QuantityFulfilled,QuantityOutstanding,UoM,AvailableFrom,AvailableTo,ItemCondition,DeliveryAvailable,ListingStatus,CallingAction,CallingController")] AvailableListingAddView availableListing)
        {
            if (ModelState.IsValid)
            {
                AvailableListingHelpers.CreateAvailableListingFromAvailableListingAddView(availableListing, User);

                return RedirectToAction(availableListing.CallingAction, availableListing.CallingController);
            }

            return View(availableListing);
        }

        // GET: AvailableListings/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailableListingEditView availableListing = AvailableListingEditHelpers.GetAvailableListingEditView(db, id.Value);
            if (availableListing == null)
            {
                return HttpNotFound();
            }
            return View(availableListing);
        }

        // POST: AvailableListings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AvailableListingEditView availableListing)
        {
            if (ModelState.IsValid)
            {
                //If the 'Submit' button pressed then update tables, else leave as are so that on reload it takes original values once again.
                if (Request.Form["submitbutton"] != null)
                {
                    //Update tables
                    AvailableListingHelpers.UpdateAvailableListingFromAvailableListingEditView(db, availableListing);

                    return RedirectToAction("Available", "ManageListings");
                }

                return RedirectToAction("Edit");
            }

            //rebuild the missing details before returning to screen to show errors
            AvailableListing listing = AvailableListingHelpers.GetAvailableListing(db, availableListing.ListingId);
            availableListing.ListingAppUser = AppUserHelpers.GetAppUser(db, listing.ListingOriginatorAppUserId);
            availableListing.ListingBranchDetails = BranchHelpers.GetBranch(db, listing.ListingOriginatorAppUserId);

            return View(availableListing);
        }

        //// GET: AvailableListings/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AvailableListing availableListing = db.AvailableListings.Find(id);
        //    if (availableListing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(availableListing);
        //}

        //// POST: AvailableListings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    AvailableListing availableListing = db.AvailableListings.Find(id);
        //    db.AvailableListings.Remove(availableListing);
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

        public ActionResult SubmitAvailableOffer(Guid? listingId, decimal? offerQuantity)
        {
            if (listingId.HasValue && offerQuantity.HasValue)
            {
                if (offerQuantity > 0)
                {
                    AvailableListing availableListing = AvailableListingHelpers.GetAvailableListing(db, listingId.Value);

                    OfferHelpers.CreateOfferForAvailable(db, User, availableListing, offerQuantity.Value);
                }

                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        #endregion
    }
}
