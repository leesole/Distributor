using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.ItemEnums;

namespace Distributor.Helpers
{
    public static class RequirementListingHelpers
    {
        #region Get

        public static RequirementListing GetRequirementListing(Guid listingId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RequirementListing requirement = GetRequirementListing(db, listingId);
            db.Dispose();
            return requirement;
        }

        public static RequirementListing GetRequirementListing(ApplicationDbContext db, Guid listingId)
        {
            return db.RequirementListings.Find(listingId);
        }
        
        public static List<RequirementListing> GetAllRequirementListings()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<RequirementListing> list = GetAllRequirementListings(db);
            db.Dispose();
            return list;
        }

        public static List<RequirementListing> GetAllRequirementListings(ApplicationDbContext db)
        {
            return db.RequirementListings.ToList();
        }

        public static List<RequirementListing> GetAllRequirementListingsForBranchUser(BranchUser branchUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<RequirementListing> list = GetAllRequirementListingsForBranchUser(db, branchUser);
            db.Dispose();
            return list;
        }

        public static List<RequirementListing> GetAllRequirementListingsForBranchUser(ApplicationDbContext db, BranchUser branchUser)
        {
            List<RequirementListing> allRequirementsListingForBranchUser = (from bu in db.BranchUsers
                                                                            from rl in db.RequirementListings
                                                                            where (bu.UserId == rl.ListingOriginatorAppUserId && bu.BranchId == rl.ListingOriginatorBranchId && bu.CompanyId == rl.ListingOriginatorCompanyId 
                                                                            && (rl.ListingStatus == ItemRequiredListingStatus.Open || rl.ListingStatus == ItemRequiredListingStatus.Partial))
                                                                            select rl).ToList();

            return allRequirementsListingForBranchUser;
        }

        #endregion

        #region Create

        public static RequirementListing CreateRequirementListing(RequirementListing requirementListing, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RequirementListing newRequirementListing = CreateRequirementListing(db, requirementListing, user);
            db.Dispose();
            return newRequirementListing;
        }

        public static RequirementListing CreateRequirementListing(ApplicationDbContext db, RequirementListing requirementListing, IPrincipal user)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);

            requirementListing.CampaignId = Guid.NewGuid();
            requirementListing.ListingOriginatorAppUserId = branchUser.UserId;
            requirementListing.ListingOriginatorBranchId = branchUser.BranchId;
            requirementListing.ListingOriginatorCompanyId = branchUser.CompanyId;
            requirementListing.ListingOriginatorDateTime = DateTime.Now;
            requirementListing.ListingStatus = ItemRequiredListingStatus.Open;

            db.RequirementListings.Add(requirementListing);
            db.SaveChanges();

            return requirementListing;
        }

#endregion
    }

    public static class RequirementListingGeneralInfoHelpers
    {
        #region Get

        public static List<RequirementListingGeneralInfoView> GetAllRequirementListingsGeneralInfoView()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<RequirementListingGeneralInfoView> list = GetAllRequirementListingsGeneralInfoView(db);
            db.Dispose();
            return list;
        }

        public static List<RequirementListingGeneralInfoView> GetAllRequirementListingsGeneralInfoView(ApplicationDbContext db)
        {
            List<RequirementListingGeneralInfoView> allRequirementListingsGeneralInfoView = new List<RequirementListingGeneralInfoView>();

            List<RequirementListing> allRequirementListings = RequirementListingHelpers.GetAllRequirementListings(db);

            foreach (RequirementListing requirementListing in allRequirementListings)
            {
                RequirementListingGeneralInfoView requirementListingGeneralInfoView = new RequirementListingGeneralInfoView()
                {
                    RequirementListing = requirementListing
                };

                allRequirementListingsGeneralInfoView.Add(requirementListingGeneralInfoView);
            }

            return allRequirementListingsGeneralInfoView;
        }

        #endregion
    }

    public static class RequirementListingManageHelpers
    {
        #region Get

        public static List<RequirementListingManageView> GetAllRequirementListingsManageViewForUserBranch(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<RequirementListingManageView> list = GetAllRequirementListingsManageViewForUserBranch(db, user);
            db.Dispose();
            return list;
        }

        public static List<RequirementListingManageView> GetAllRequirementListingsManageViewForUserBranch(ApplicationDbContext db, IPrincipal user)
        {
            List<RequirementListingManageView> allRequirementListingsManageView = new List<RequirementListingManageView>();

            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);
            List<RequirementListing> allRequirementListingsForBranchUser = RequirementListingHelpers.GetAllRequirementListingsForBranchUser(db, branchUser);

            foreach (RequirementListing requirementListingForBranchUser in allRequirementListingsForBranchUser)
            {
                RequirementListingManageView requirementListingManageView = new RequirementListingManageView()
                {
                    RequirementListing = requirementListingForBranchUser
                };

                allRequirementListingsManageView.Add(requirementListingManageView);
            }

            return allRequirementListingsManageView;
        }

        #endregion
    }
}