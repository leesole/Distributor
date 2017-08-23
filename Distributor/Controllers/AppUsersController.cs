﻿using System;
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
using Distributor.Extenstions;
using static Distributor.Enums.EntityEnums;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using static Distributor.Enums.UserTaskEnums;
using Microsoft.AspNet.Identity.Owin;

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

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: AppUsers/Create
        public ActionResult Create()
        {
            Branch userBranch = BranchHelpers.GetCurrentBranchForUser(AppUserHelpers.GetGuidFromUserGetAppUserId(User.Identity.GetAppUserId()));

            //DropDowns
            ViewBag.BranchList = ControlHelpers.AllBranchesForCompanyListDropDown(userBranch.CompanyId, userBranch.BranchId);
            ViewBag.UserRoleList = ControlHelpers.UserRoleEnumListDropDown();
            ViewBag.EntityStatusList = ControlHelpers.EntityStatusEnumListDropDown();

            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppUserView model)
        {
            if (ModelState.IsValid)
            {
                //initialise the task creation flags
                bool createUserOnHoldTask = false;

                //Retrieve Branch
                Branch branch = BranchHelpers.GetBranch(db, model.SelectedBranchId.Value);

                //Create a new AppUser then write here
                AppUser appUser = AppUserHelpers.CreateAppUser(model.FirstName, model.LastName, branch.BranchId, model.EntityStatus);

                BranchUser branchUser = null;

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, AppUserId = appUser.AppUserId, FullName = model.FirstName + " " + model.LastName, CurrentUserRole = model.UserRole };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //set on-hold task flag
                    if (model.EntityStatus == EntityStatusEnum.OnHold)
                        createUserOnHoldTask = true;

                    //Now Update related entities
                    //BranchUser - set the status as ACTIVE as the link is active even though the entities linked are not.
                    branchUser = BranchUserHelpers.CreateBranchUser(appUser.AppUserId, branch.BranchId, branch.CompanyId, model.UserRole, EntityStatusEnum.Active);

                    //Task creation
                    if (createUserOnHoldTask)
                        UserTaskHelpers.CreateUserTask(TaskTypeEnum.UserOnHold, "New user on hold, awaiting administrator/manager activation", appUser.AppUserId, appUser.AppUserId, EntityStatusEnum.Active);

                    return RedirectToAction("UserAdmin", "Admin");
                }

                //Delete the appUser account as this has not gone through
                AppUserHelpers.DeleteAppUser(appUser.AppUserId);
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form - set up the drop downs dependant on what was there originally from the model
            Branch userBranch = BranchHelpers.GetCurrentBranchForUser(AppUserHelpers.GetGuidFromUserGetAppUserId(User.Identity.GetAppUserId()));
            //DropDown
            ViewBag.BranchList = ControlHelpers.AllBranchesForCompanyListDropDown(userBranch.CompanyId, userBranch.BranchId);
            ViewBag.UserRoleList = ControlHelpers.UserRoleEnumListDropDown();
            ViewBag.EntityStatusList = ControlHelpers.EntityStatusEnumListDropDown();

            return View(model);
        }

        //// POST: AppUsers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(AppUserView model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //    }

        //    Branch branch = BranchHelpers.GetCurrentBranchForUser(AppUserHelpers.GetGuidFromUserGetAppUserId(User.Identity.GetAppUserId()));
        //    //DropDown
        //    ViewBag.BranchList = ControlHelpers.AllBranchesForCompanyListDropDown(branch.CompanyId, branch.BranchId);
        //    ViewBag.UserRoleList = ControlHelpers.UserRoleEnumListDropDown();
        //    ViewBag.EntityStatusList = ControlHelpers.EntityStatusEnumListDropDown();

        //    return View(model);
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
