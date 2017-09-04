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
    public class CampaignsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Campaigns
        //public ActionResult Index()
        //{
        //    return View(db.Campaigns.ToList());
        //}

        // GET: Campaigns/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignEditView campaign = CampaignEditHelpers.GetCampaignEditView(db, id.Value);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        // GET: Campaigns/Create
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

            CampaignAddView model = new CampaignAddView()
            {
                CallingAction = callingAction,
                CallingController = callingController
            };

            return View(model);
        }

        // POST: Campaigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,StrapLine,Description,Image,ImageLocation,Website,CampaignStartDateTime,CampaignEndDateTime,LocationName,LocationAddressLine1,LocationAddressLine2,LocationAddressLine3,LocationAddressTownCity,LocationAddressCounty,LocationAddressPostcode,LocationTelephoneNumber,LocationEmail,LocationContactName,CallingAction,CallingController")] CampaignAddView campaign)
        { 

            if (ModelState.IsValid)
            {
                CampaignHelpers.CreateCampaignFromCampaignAddView(campaign, User);
                
                return RedirectToAction(campaign.CallingAction, campaign.CallingController);
            }

            return View(campaign);
        }

        // GET: Campaigns/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignEditView campaign = CampaignEditHelpers.GetCampaignEditView(db, id.Value);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        // POST: Campaigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CampaignEditView campaign)
        {
            if (ModelState.IsValid)
            {
                //If the 'Submit' button pressed then update tables, else leave as are so that on reload it takes original values once again.
                if (Request.Form["submitbutton"] != null)
                {
                    //Update tables
                    CampaignHelpers.UpdateCampaignFromCampaignEditView(db, campaign);

                    return RedirectToAction("Campaigns", "ManageListings");
                }

                return RedirectToAction("Edit");
            }

            //rebuild the missing details before returning to screen to show errors
            Campaign campaignDetails = CampaignHelpers.GetCampaign(db, campaign.CampaignId);
            campaign.CampaignAppUser = AppUserHelpers.GetAppUser(db, campaignDetails.CampaignOriginatorAppUserId);
            campaign.CampaignBranchDetails = BranchHelpers.GetBranch(db, campaignDetails.CampaignOriginatorBranchId);

            return View(campaign);
        }

        //// GET: Campaigns/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Campaign campaign = db.Campaigns.Find(id);
        //    if (campaign == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(campaign);
        //}

        //// POST: Campaigns/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Campaign campaign = db.Campaigns.Find(id);
        //    db.Campaigns.Remove(campaign);
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
