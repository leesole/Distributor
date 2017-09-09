﻿using Distributor.Extenstions;
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
            List<Campaign> campaignsForUser = CampaignHelpers.GetAllCampaignsForUser(db, appUser.AppUserId);
            dashboardView.CampaignList = campaignsForUser;

            List<Campaign> campaignsRecentList = CampaignHelpers.GetAllRecentCampaigns(db, appUser.AppUserId);
            dashboardView.CampaignRecentList = campaignsRecentList;

            List<RequirementListing> requirementListingsForUser = RequirementListingHelpers.GetAllRequirementListingsForUser(db, appUser.AppUserId);
            dashboardView.RequirementListingList = requirementListingsForUser;

            List<RequirementListing> requirementListingRecentList = RequirementListingHelpers.GetAllRequirementListingsForPastXDays(db, 14);
            dashboardView.RequirementListingRecentList = requirementListingRecentList;

            List<AvailableListing> availableListingsForUser = AvailableListingHelpers.GetAllAvailableListingsForUser(db, appUser.AppUserId);
            dashboardView.AvailableListingList = availableListingsForUser;

            List<AvailableListing> availableListingRecentList = AvailableListingHelpers.GetAllAvailableListingsForPastXDays(db, 14);
            dashboardView.AvailableListingRecentList = availableListingRecentList;

            List<Offer> offersForUser = OfferHelpers.GetAllOffersForUser(db, appUser.AppUserId);
            dashboardView.OfferList = offersForUser;

            List<Order> ordersForUser = OrderHelpers.GetAllOrdersForUser(db, appUser.AppUserId);
            dashboardView.OrderList = ordersForUser;

            //get listings for admin areas if this user is not a 'User' - i.e. is Manager, Admin etc.
            if (user.Identity.GetCurrentUserRole() != "User")
            {
                List<UserTask> tasksForUser = UserTaskHelpers.GetUserTasksForUser(db, appUser.AppUserId);
                //get list of 'actions' once written

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