using Distributor.Helpers;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        // GET: Task
        public ActionResult Details(Guid? userTaskId)
        {
            if (userTaskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationDbContext db = new ApplicationDbContext();

            //Get current user for building the Usertaskview of selected record
            AppUser appUser = AppUserHelpers.GetAppUser(db, User);

            //Get UserTaskView of selected UserTask record
            UserTaskView userTaskView = UserTaskViewHelpers.GetUserTaskForUserView(db, appUser.AppUserId, userTaskId.Value);

            //Get the requestor appuser details and branch details
            AppUser createdByAppUser = AppUserHelpers.GetAppUser(userTaskView.CreatedBy.AppUserId);
            Branch createdByAppUserCurrentBranch = BranchHelpers.GetCurrentBranchForUser(db, userTaskView.CreatedBy.AppUserId);

            //If this is a on-hold user then get the current user role
            BranchUser branchUser = null;

            if (userTaskView.AppUserReference != null)
            {
                branchUser = BranchUserHelpers.GetBranchUser(db, userTaskView.AppUserReference.AppUserId, userTaskView.AppUserReference.CurrentBranchId);
            }

            //Build the view model
            UserTaskFullView userTaskFullView = new UserTaskFullView();
            userTaskFullView.UserTaskView = userTaskView;
            if (branchUser != null)
                userTaskFullView.BranchUserUserRole = branchUser.UserRole;
            userTaskFullView.CreatedByAppUser = createdByAppUser;
            userTaskFullView.CreatedByAppUserCurrentBranch = createdByAppUserCurrentBranch;
            
            if (userTaskView.AppUserReference != null)
                ViewBag.EntityStatusUserRole = EnumHelpers.GetDescription((EntityStatusEnum)userTaskFullView.UserTaskView.AppUserReference.EntityStatus);
            else
                ViewBag.EntityStatusUserRole = "";

            if (userTaskView.BranchReference != null)
                ViewBag.EntityStatusBranchStatus = EnumHelpers.GetDescription((EntityStatusEnum)userTaskFullView.UserTaskView.BranchReference.EntityStatus);
            else
                ViewBag.EntityStatusBranchStatus = "";

            ViewBag.EntityStatusCreatedByUserRole = EnumHelpers.GetDescription((EntityStatusEnum)userTaskFullView.CreatedByAppUser.EntityStatus);
            ViewBag.EntityStatusCreatedByUserBranchStatus = EnumHelpers.GetDescription((EntityStatusEnum)userTaskFullView.CreatedByAppUserCurrentBranch.EntityStatus);

            return View(userTaskFullView);
        }
    }
}