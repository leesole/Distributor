using Distributor.Extenstions;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Distributor.Helpers
{
    public static class DashboardHelpers
    {
        public static DashboardView GetDashboardView()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            DashboardView dashboardView = GetDashboardView(db);
            db.Dispose();
            return dashboardView;
        }

        public static DashboardView GetDashboardView(ApplicationDbContext db)
        {
           //initialise the view
            DashboardView dashboardView = new DashboardView();

            //get the campaigns and listings for this user
            List<Campaign> campaigns = CampaignHelpers.GetAllCampaigns(db);
            dashboardView.CampaignList = campaigns;

            List<RequirementListing> requirementListings = RequirementListingHelpers.GetAllRequirementListings(db);
            dashboardView.RequirementListingList = requirementListings;

            return dashboardView;
        }

        public static DashboardView GetDashboardViewLogin(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            DashboardView dashboardView = GetDashboardViewLogin(db, user);
            db.Dispose();
            return dashboardView;
        }

        public static DashboardView GetDashboardViewLogin(ApplicationDbContext db, IPrincipal user)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, user);

            //initialise the view
            DashboardView dashboardView = new DashboardView();

            //get the campaigns and listings for this user
            List<Campaign> campaignsForUser = CampaignHelpers.GetAllCampaignsForUser(db, appUser.AppUserId);
            dashboardView.CampaignList = campaignsForUser;

            List<RequirementListing> requirementListingsForUser = RequirementListingHelpers.GetAllRequirementListingsForUser(db, appUser.AppUserId);
            dashboardView.RequirementListingList = requirementListingsForUser;

            List<AvailableListing> availableListingsForUser = AvailableListingHelpers.GetAllAvailableListingsForUser(db, appUser.AppUserId);
            dashboardView.AvailableListingList = availableListingsForUser;

            //get listings for admin areas if this user is not a 'User' - i.e. is Manager, Admin etc.
            if (user.Identity.GetCurrentUserRole() != "User")
            {
                List<UserTask> tasksForUser = UserTaskHelpers.GetUserTasksForUser(db, appUser.AppUserId);
                //get list of 'actions' once written

                dashboardView.UserTaskList = tasksForUser;
            }

            return dashboardView;
        }
    }
}