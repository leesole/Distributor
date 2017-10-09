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
using static Distributor.Enums.GeneralEnums;

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
        public ActionResult Details(Guid? offerId, bool showHistory)
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
            ViewBag.ShowHistory = showHistory;

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

        public ActionResult History()
        {
            List<OfferManageView> model = OfferManageHelpers.GetAllOffersManageView(User, true);

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
