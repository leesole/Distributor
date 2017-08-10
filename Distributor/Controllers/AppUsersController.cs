using System;
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
    public class AppUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: AppUsers
        //public ActionResult Index()
        //{
        //    return View(db.AppUsers.ToList());
        //}

        //// GET: AppUsers/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AppUser appUser = db.AppUsers.Find(id);
        //    if (appUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(appUser);
        //}

        //// GET: AppUsers/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AppUsers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "AppUserId,FirstName,LastName,EntityStatus,CurrentBranchId")] AppUser appUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        appUser.AppUserId = Guid.NewGuid();
        //        db.AppUsers.Add(appUser);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(appUser);
        //}

        // GET: AppUsers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppUserId,FirstName,LastName,EntityStatus,CurrentBranchId")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appUser);
        }

        //// GET: AppUsers/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AppUser appUser = db.AppUsers.Find(id);
        //    if (appUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(appUser);
        //}

        //// POST: AppUsers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    AppUser appUser = db.AppUsers.Find(id);
        //    db.AppUsers.Remove(appUser);
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
