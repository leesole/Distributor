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
using static Distributor.Enums.UserTaskEnums;

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
            UserTaskView userTaskView = null;
            UserTaskFullView userTaskFullView = new UserTaskFullView()
            {
                UserTaskView = userTaskView
            };

            try  //try helps with issues, if no records for example then we are left with null userTaskView instead of error
            {
                userTaskView = UserTaskViewHelpers.GetUserTaskForUserView(db, appUser.AppUserId, userTaskId.Value);

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
                userTaskFullView = new UserTaskFullView();
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

                ViewBag.UserTaskUserRole = EnumHelpers.GetDescription((UserRoleEnum)branchUser.UserRole);
            }
            catch { }

            return View(userTaskFullView);
        }

        #region data manipulation

        public ActionResult ApproveTask(Guid? userTaskId)
        {
            if (userTaskId.HasValue)
            {
                UserTask userTask = UserTaskHelpers.GetUserTask(userTaskId.Value);
                
                switch (userTask.TaskType)
                {
                    case TaskTypeEnum.UserOnHold:  //Make AppUser active
                        AppUserHelpers.UpdateEntityStatus(userTask.ReferenceKey, EntityStatusEnum.Active);
                        break;
                    case TaskTypeEnum.BranchOnHold:  //Make Branch active
                        BranchHelpers.UpdateEntityStatus(userTask.ReferenceKey, EntityStatusEnum.Active);
                        break;
                }

                //close the Task
                UserTaskHelpers.UpdateEntityStatus(userTask.UserTaskId, EntityStatusEnum.Closed);

                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        public ActionResult CancelTask(Guid? userTaskId)
        {
            if (userTaskId.HasValue)
            {
                UserTask userTask = UserTaskHelpers.GetUserTask(userTaskId.Value);

                switch (userTask.TaskType)
                {
                    case TaskTypeEnum.UserOnHold:  //Make AppUser inactive
                        AppUserHelpers.UpdateEntityStatus(userTask.ReferenceKey, EntityStatusEnum.Inactive);
                        break;
                    case TaskTypeEnum.BranchOnHold:  //Make Branch inactive
                        BranchHelpers.UpdateEntityStatus(userTask.ReferenceKey, EntityStatusEnum.Inactive);
                        break;
                }

                //close the Task
                UserTaskHelpers.UpdateEntityStatus(userTask.UserTaskId, EntityStatusEnum.Closed);

                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }

        #endregion
    }
}