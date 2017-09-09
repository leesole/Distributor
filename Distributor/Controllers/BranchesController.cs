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
    [Authorize]
    public class BranchesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Branches
        //public ActionResult Index()
        //{
        //    return View(db.Branches.ToList());
        //}

        //// GET: Branches/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Branch branch = db.Branches.Find(id);
        //    if (branch == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(branch);
        //}

        // GET: Branches/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = CompanyHelpers.GetCompanyForUser(User).CompanyId;
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BranchId,CompanyId,BranchName,BusinessType,AddressLine1,AddressLine2,AddressLine3,AddressTownCity,AddressCounty,AddressPostcode,TelephoneNumber,Email,ContactName,EntityStatus")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                BranchHelpers.CreateBranch(branch);
                return RedirectToAction("BranchAdmin", "Admin");
            }

            return View(branch);
        }

        //// GET: Branches/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Branch branch = db.Branches.Find(id);
        //    if (branch == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(branch);
        //}

        //// POST: Branches/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "BranchId,CompanyId,BranchName,BusinessType,AddressLine1,AddressLine2,AddressLine3,AddressTownCity,AddressCounty,AddressPostcode,TelephoneNumber,Email,ContactName,EntityStatus")] Branch branch)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(branch).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(branch);
        //}

        //// GET: Branches/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Branch branch = db.Branches.Find(id);
        //    if (branch == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(branch);
        //}

        //// POST: Branches/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Branch branch = db.Branches.Find(id);
        //    db.Branches.Remove(branch);
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
