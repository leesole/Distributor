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
using System.IO;

namespace Distributor.Controllers
{
    [Authorize]
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
            CampaignEditView campaign = CampaignEditHelpers.GetCampaignEditView(db, id.Value, User);
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
        public ActionResult Create([Bind(Include = "Name,StrapLine,Description,Image,ImageLocation,Website,CampaignStartDateTime,CampaignEndDateTime,LocationName,LocationAddressLine1,LocationAddressLine2,LocationAddressLine3,LocationAddressTownCity,LocationAddressCounty,LocationAddressPostcode,LocationTelephoneNumber,LocationEmail,LocationContactName,CallingAction,CallingController")] CampaignAddView campaign, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/images"), pic);
                    // file is uploaded
                    //file.SaveAs(path);

                    //campaign.ImageLocation = path;

                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();

                        campaign.Image = array;
                    }

                }

                CampaignHelpers.CreateCampaignFromCampaignAddView(campaign, User);
                
                return RedirectToAction(campaign.CallingAction, campaign.CallingController);
            }

            return View(campaign);
        }

        // GET: Campaigns/Edit/5
        public ActionResult Edit(Guid? id, bool showHistory)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignEditView campaign = CampaignEditHelpers.GetCampaignEditView(db, id.Value, User);
            if (campaign == null)
            {
                return HttpNotFound();
            }

            ViewBag.ShowHistory = showHistory;

            return View(campaign);
        }

        // POST: Campaigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CampaignId,Name,StrapLine,Description,Image,ImageLocation,Website,CampaignStartDateTime,CampaignEndDateTime,LocationName,LocationAddressLine1,LocationAddressLine2,LocationAddressLine3,LocationAddressTownCity,LocationAddressCounty,LocationAddressPostcode,LocationTelephoneNumber,LocationEmail,LocationContactName,EntityStatus")]CampaignEditView campaign)
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

        public ActionResult History()
        {
            List<CampaignManageView> model = CampaignManageHelpers.GetAllCampaignsManageView(User, true);
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
    }
}
