using Distributor.Extenstions;
using Distributor.Helpers;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Distributor.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Tasks()
        {
            AppUser appUser = AppUserHelpers.GetAppUser(User);
            
            List<UserTaskView> userTasksForUserView = UserTaskViewHelpers.GetUserTasksForUserView(appUser.AppUserId);
            return View(userTasksForUserView);
        }

        public ActionResult UserAdmin()
        {
            List<UserAdminView> userAdminViewForUser = UserAdminHelpers.GetUserAdminViewListForUser(User);
            ViewBag.CurrentUserId = AppUserHelpers.GetAppUser(User).AppUserId;
            //TempData["OriginalModel"] = userAdminViewForUser;
            return View(userAdminViewForUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserAdmin(List<UserAdminView> userAdminViewForUser)
        {
            if (ModelState.IsValid)
            {
                //var originalModel = TempData["OriginalModel"];

                if (Request.Form["submitbutton"] != null)
                {
                    //Update tables
                    UserAdminHelpers.UpdateUsersFromUserAdminView(userAdminViewForUser, User);
                }

                return RedirectToAction("UserAdmin");
            }

            return View(userAdminViewForUser);
        }

        public ActionResult BranchAdmin()
        {
            List<BranchAdminView> branchesAdminView = BranchAdminHelpers.GetBranchAdminViewList(User);
            ViewBag.CurrentUserId = AppUserHelpers.GetAppUser(User).AppUserId;
            return View(branchesAdminView);
        }

        // POST: CompanyAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BranchAdmin(List<BranchAdminView> branchesAdminView)
        {
            if (ModelState.IsValid)
            {
                //If the 'Submit' button pressed then update tables, else leave as are so that on reload it takes original values once again.
                if (Request.Form["submitbutton"] != null)
                {
                    //Update tables
                    BranchAdminHelpers.UpdateBranchesFromBranchAdminView(branchesAdminView, User);
                }

                return RedirectToAction("BranchAdmin");
            }

            return View(branchesAdminView);
        }

        public ActionResult CompanyAdmin()
        {
            //validate that you cannot manually get into here without the right level, ie ADMIN, SuperAdmin
            if (User.Identity.GetCurrentUserRole() != "Admin" && User.Identity.GetCurrentUserRole() != "SuperUser")
                return RedirectToAction("Index", "Home");
            else
            {
                CompanyAdminView companyAdminView = CompanyAdminHelpers.GetCompanyAdminView(User);
                return View(companyAdminView);
            }
        }

        // POST: CompanyAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyAdmin(CompanyAdminView companyAdminView)
        {
            if (ModelState.IsValid)
            {
                //If the 'Submit' button pressed then update tables, else leave as are so that on reload it takes original values once again.
                if (Request.Form["submitbutton"] != null)
                {
                    CompanyAdminHelpers.UpdateCompanyFromCompanyAdminView(companyAdminView);
                }

                return RedirectToAction("CompanyAdmin");
            }
            return View(companyAdminView);
        }

        #region Data Manipulation

        #region UserAdmin

        [HttpPost]
        public ActionResult ChangeCurrentBranchForUser(List<UserAdminView> modelDetails, Guid appUserId, Guid branchId)
        {
            //modelDetails coming through as blank so for now just update this value immediatetly, return and refresh data
            AppUserHelpers.UpdateCurrentBranchId(appUserId, branchId);

            return Json(new { success = true });
        }

        #endregion

        #endregion
    }
}