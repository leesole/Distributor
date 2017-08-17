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
            List<UserAdminView> userAdminViewForUser = UserAdminHelpers.GetUserAdminViewListForUser(User);
            return View(userAdminViewForUser);
        }

        public ActionResult BranchAdmin()
        {
            List<BranchAdminView> branchAdminView = BranchAdminHelpers.GetBranchAdminViewList(User);
            return View(branchAdminView);
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
    }
}