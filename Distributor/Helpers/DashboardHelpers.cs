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
        #region Get

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
            List<Campaign> campaignsForUser = CampaignHelpers.GetAllCampaignsForUser(db, appUser.AppUserId, false);
            dashboardView.CampaignList = campaignsForUser;

            List<Campaign> campaignsDashboardList = CampaignHelpers.GetAllDashboardFilteredCampaigns(db, appUser.AppUserId);
            dashboardView.CampaignDashboardList = campaignsDashboardList;

            List<RequirementListing> requirementListingsForUser = RequirementListingHelpers.GetAllRequirementListingsForUser(db, appUser.AppUserId, false);
            dashboardView.RequirementListingList = requirementListingsForUser;

            List<RequirementListing> requirementListingDashboardList = RequirementListingHelpers.GetAllDashboardFilteredRequirementListings(db, appUser.AppUserId);
            dashboardView.RequirementListingDashboardList = requirementListingDashboardList;

            List<AvailableListing> availableListingsForUser = AvailableListingHelpers.GetAllAvailableListingsForUser(db, appUser.AppUserId, false);
            dashboardView.AvailableListingList = availableListingsForUser;

            List<AvailableListing> availableListingDashboardList = AvailableListingHelpers.GetAllDashboardFilteredAvailableListings(db, appUser.AppUserId);
            dashboardView.AvailableListingDashboardList = availableListingDashboardList;

            List<Offer> offersForUser = OfferHelpers.GetAllOffersForUser(db, appUser.AppUserId, false);
            dashboardView.OfferList = offersForUser;

            List<Order> ordersForUser = OrderHelpers.GetAllOrdersForUser(db, appUser.AppUserId, false);
            dashboardView.OrderList = ordersForUser;

            //get listings for admin areas if this user is not a 'User' - i.e. is Manager, Admin etc.
            if (user.Identity.GetCurrentUserRole() != "User")
            {
                List<UserTask> tasksForUser = UserTaskHelpers.GetUserTasksForUser(db, appUser.AppUserId);
                //LSLSLS get list of 'actions' once written

                dashboardView.UserTaskList = tasksForUser;
            }

            return dashboardView;
        }

        #endregion

        #region Processes

        public static decimal GetRequirementsTotalFromDashboardView(DashboardView view)
        {
            decimal value = 0;

            foreach (RequirementListing requirement in view.RequirementListingList)
                value += requirement.QuantityRequired;

            return value;
        }

        public static decimal GetRequirementsFulfilledFromDashboardView(DashboardView view)
        {
            decimal value = 0;

            foreach (RequirementListing requirement in view.RequirementListingList)
                value += requirement.QuantityFulfilled;

            return value;
        }

        public static decimal GetRequirementsOutstandingFromDashboardView(DashboardView view)
        {
            decimal value = 0;

            foreach (RequirementListing requirement in view.RequirementListingList)
                value += requirement.QuantityOutstanding;

            return value;
        }

        public static decimal GetAvailableTotalFromDashboardView(DashboardView view)
        {
            decimal value = 0;

            foreach (AvailableListing available in view.AvailableListingList)
                value += available.QuantityRequired;

            return value;
        }

        public static decimal GetAvailableFulfilledFromDashboardView(DashboardView view)
        {
            decimal value = 0;

            foreach (AvailableListing available in view.AvailableListingList)
                value += available.QuantityFulfilled;

            return value;
        }

        public static decimal GetAvailableOutstandingFromDashboardView(DashboardView view)
        {
            decimal value = 0;

            foreach (AvailableListing available in view.AvailableListingList)
                value += available.QuantityOutstanding;

            return value;
        }

        #endregion
    }
}