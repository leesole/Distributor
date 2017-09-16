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
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        public ActionResult Index()
        {
            GroupEditView model = GroupViewHelpers.GetGroupEditViewForUser(db, AppUserHelpers.GetAppUserIdFromUser(User));
            return View(model);

            //This view will do everything.
            //List all groups you belong to
            //List all your 'friends' groups
            //List the 'requests' to join your group for approval
            //Allow you to 'add' a new group
            //Allow you to remove 'your' group (you are group admin)
            //Allow you to 'request' a friend join your group (on them saying yes it automatically adds them)
            //Allow you to 'join' a friends group (on them saying yes it automatically adds you)

            //PS Need to add the 'friend request' stuff also. - i.e.
            //Friend table that holds type (User, branch, company) and Id.
            //- on company admin it will show company friends and give 'admin' the ability to add/remove etc..
            //- on branch admin it will show branch friends and give 'admin' & 'manager' the ability to add/remove etc.
            //- on user it will show user friends and give all ability to add/remove etc..
            //.....therefore 'add friend' on listings page highlighting company/branch/user need to be displayed for those with right access.
            //.....this will then go off for approval
        }

        // GET: Groups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        //first 4 variables come from the +Button on the general info screens.
        public ActionResult Create(LevelEnum? level, Guid? ofReferenceId, Guid? byReferenceId, Guid? byAppUserId, Guid? appUserId)
        {
            if (appUserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GroupAddView model = GroupViewHelpers.GetGroupAddView(level, ofReferenceId, byReferenceId, byAppUserId, appUserId.Value);

            return View(model);
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupAddView model)
        {
            if (ModelState.IsValid)
            {
                GroupHelpers.CreateGroupFromGroupAddView(db, model, User);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult GetMemberList(string groupName, string memberType)
        {
            GroupAddView model = GroupViewHelpers.GetGroupAddView(groupName, memberType, User);

            return PartialView("AddGroupMembers", model);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,Name,GroupOriginatorAppUserId,GroupOriginatorDateTime")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
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
