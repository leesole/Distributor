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

        public AppUser AppUserDetails { get; set; }

        public Branch BranchDetails { get; set; }
    }
}