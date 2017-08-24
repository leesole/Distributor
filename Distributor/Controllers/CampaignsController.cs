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

        //// GET: Campaigns/Details/5
        //public ActionResult Details(Guid? id)
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

        // GET: Campaigns/Create
        public ActionResult Create()
        {
            string[] callingUrlSegments = Request.UrlReferrer.Segments.Select(x => x.TrimEnd('/')).ToArray();
            string callingController = callingUrlSegments[callingUrlSegments.Count() - 2];
            string callingView = callingUrlSegments[callingUrlSegments.Count() - 1];

            ViewBag.CallingController = callingController;
            ViewBag.CallingView = callingView;

            return View();
        }

        // POST: Campaigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CampaignId,Name,StrapLine,Description,Image,ImageLocation,Website,CampaignStartDateTime,CampaignEndDateTime,LocationName,LocationAddressLine1,LocationAddressLine2,LocationAddressLine3,LocationAddressTownCity,LocationAddressCounty,LocationAddressPostcode,LocationTelephoneNumber,LocationEmail,LocationContactName,EntityStatus,CampaignOriginatorAppUserId,CampaignOriginatorBranchId,CampaignOriginatorCompanyId,CampaignOriginatorDateTime")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                CampaignHelpers.CreateCampaign(campaign, User);
                return RedirectToAction("Campaigns", "GeneralInfo");
            }

            return View(campaign);
        }

        //// GET: Campaigns/Edit/5
        //public ActionResult Edit(Guid? id)
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

        //// POST: Campaigns/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "CampaignId,Name,StrapLine,Description,Image,ImageLocation,Website,CampaignStartDateTime,CampaignEndDateTime,LocationName,LocationAddressLine1,LocationAddressLine2,LocationAddressLine3,LocationAddressTownCity,LocationAddressCounty,LocationAddressPostcode,LocationTelephoneNumber,LocationEmail,LocationContactName,EntityStatus,CampaignOriginatorAppUserId,CampaignOriginatorBranchId,CampaignOriginatorCompanyId,CampaignOriginatorDateTime")] Campaign campaign)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(campaign).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(campaign);
        //}

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
