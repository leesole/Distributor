﻿using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Distributor.ViewModels
{
    public class DashboardView
    {
        public List<UserTask> UserTaskList { get; set; }
        //action list
        public List<Campaign> CampaignList { get; set; }
        public List<Campaign> CampaignDashboardList { get; set; }
        public List<RequirementListing> RequirementListingList { get; set; }
        public List<RequirementListing> RequirementListingDashboardList { get; set; }
        public List<AvailableListing> AvailableListingList { get; set; }
        public List<AvailableListing> AvailableListingDashboardList { get; set; }
        public List<Offer> OfferList { get; set; }
        public List<Order> OrderList { get; set; }
    }
}