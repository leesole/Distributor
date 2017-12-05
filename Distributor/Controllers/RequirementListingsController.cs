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
using static Distributor.Enums.ItemEnums;

namespace Distributor.Controllers
{
    [Authorize]
    public class RequirementListingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RequirementListings/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequirementListingEditView requirementListing = RequirementListingEditHelpers.GetRequirementListingEditView(db, id.Value, User);
            if (requirementListing == null)
            {
                return HttpNotFound();
            }
            return View(requirementListing);
        }

        // GET: RequirementListings/Create
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
            ViewBag.CampaignList = ControlHelpers.AllActiveCampaignsForUserListDropDown(AppUserHelpers.GetAppUserIdFromUser(User), null);

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
        public ActionResult Create([Bind(Include = "ItemDescription,ItemType,QuantityRequired,QuantityFulfilled,QuantityOutstanding,UoM,RequiredFrom,RequiredTo,AcceptDamagedItems,AcceptOutOfDateItems,CollectionAvailable,ListingStatus,SelectedCampaignId,CallingAction,CallingController")] RequirementListingAddView requirementListing)
        {
            if (ModelState.IsValid)
            {
                RequirementListingHelpers.CreateRequirementListingFromRequirementListingAddView(db, requirementListing, User);

                return RedirectToAction(requirementListing.CallingAction, requirementListing.CallingController);
            }

            ViewBag.CampaignList = ControlHelpers.AllActiveCampaignsForUserListDropDown(AppUserHelpers.GetAppUserIdFromUser(User), null);
            return View(requirementListing);
        }

        // GET: RequirementListings/Edit/5
        public ActionResult Edit(Guid? id, bool showHistory)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequirementListingEditView requirementListing = RequirementListingEditHelpers.GetRequirementListingEditView(db, id.Value, User);
            if (requirementListing == null)
            {
                return HttpNotFound();
            }

            ViewBag.CampaignList = ControlHelpers.AllActiveCampaignsForUserListDropDown(AppUserHelpers.GetAppUserIdFromUser(User), requirementListing.SelectedCampaignId);
            ViewBag.ShowHistory = showHistory;

            return View(requirementListing);
        }

        // POST: RequirementListings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ListingId,ItemDescription,ItemType,QuantityRequired,QuantityFulfilled,QuantityOutstanding,UoM,RequiredFrom,RequiredTo,AcceptDamagedItems,AcceptOutOfDateItems,CollectionAvailable,ListingStatus,SelectedCampaignId,CampaignName,CampaignStrapLine,CampaignDescription,CampaignStartDateTime,CampaignEndDateTime")]RequirementListingEditView requirementListing)
        {
            if (ModelState.IsValid)
            {
                //If the 'Submit' button pressed then update tables, else leave as are so that on reload it takes original values once again.
                if (Request.Form["submitbutton"] != null)
                {
                    //Update tables
                    RequirementListingHelpers.UpdateRequirementListingFromRequirementListingEditView(db, requirementListing);

                    return RedirectToAction("Requirements", "ManageListings");
                }

                return RedirectToAction("Edit");
            }
            
            //rebuild the missing details before returning to screen to show errors
            RequirementListing listing = RequirementListingHelpers.GetRequirementListing(db, requirementListing.ListingId);
            requirementListing.ListingAppUser = AppUserHelpers.GetAppUser(db, listing.ListingOriginatorAppUserId);
            requirementListing.ListingBranchDetails = BranchHelpers.GetBranch(db, listing.ListingOriginatorAppUserId);

            //Rebuild campaign details as changing will change ID but no address details, so force the change now:
            if (requirementListing.SelectedCampaignId != null)
            {
                Campaign campaign = CampaignHelpers.GetCampaign(db, requirementListing.SelectedCampaignId.Value);
                requirementListing.CampaignName = campaign.Name;
                requirementListing.CampaignStrapLine = campaign.StrapLine;
                requirementListing.CampaignDescription = campaign.Description;
                requirementListing.CampaignStartDateTime = campaign.CampaignStartDateTime;
                requirementListing.CampaignEndDateTime = campaign.CampaignEndDateTime;
            }

            ViewBag.CampaignList = ControlHelpers.AllActiveCampaignsForUserListDropDown(AppUserHelpers.GetAppUserIdFromUser(User), requirementListing.SelectedCampaignId);

            return View(requirementListing);
        }

        public ActionResult History()
        {
            List<RequirementListingManageView> model = RequirementListingManageHelpers.GetAllRequirementListingsManageView(User, true);
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

        #region data manipulation

        public ActionResult SubmitRequirementOffer(Guid? listingId, decimal? offerQuantity)
        {
            if (listingId.HasValue && offerQuantity.HasValue)
            {
                if (offerQuantity > 0)
                {
                    RequirementListing requirementListing = RequirementListingHelpers.GetRequirementListing(db, listingId.Value);

                    OfferHelpers.CreateOfferForRequirement(db, User, requirementListing, offerQuantity.Value);
                }

                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        #endregion
    }
}
