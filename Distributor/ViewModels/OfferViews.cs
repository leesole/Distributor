using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Distributor.ViewModels
{
    public class OfferViews
    {
        
    }

    public class OfferManageView
    {
        public Offer OfferDetails { get; set; }

        public AvailableListing AvailableListingDetails { get; set; }

        public RequirementListing RequirementListingDetails { get; set; }

        public AppUser OfferAppUserDetails { get; set; }

        public AppUser ListingAppUserDetails { get; set; }

        public Branch OfferBranchDetails { get; set; }

        public Branch ListingBranchDetails { get; set; }

        public AppUserSettings OfferAppUserSettings { get; set; }

        public AppUserSettings ListingAppUserSettings { get; set; }
    }
}