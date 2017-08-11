using Distributor.Extenstions;
using Distributor.Helpers;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }

        public ActionResult BranchAdmin()
        {
            return View();
        }
        public ActionResult CompanyAdmin()
        {
            //validate that you cannot manually get into here without the right level, iei ADMIN, SuperAdmin
            if (User.Identity.GetCurrentUserRole() != "Admin" || User.Identity.GetCurrentUserRole() != "SuperUser")
                return Redirect(Request.QueryString["redirect"]);
            else
                return View();
        }
    }
}