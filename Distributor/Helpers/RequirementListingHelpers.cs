using Distributor.Extenstions;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public static List<RequirementListing> GetAllRequirementListingsForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<RequirementListing> list = GetAllRequirementListingsForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<RequirementListing> GetAllRequirementListingsForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<RequirementListing> allRequirementsListingForUser = (from rl in db.RequirementListings
                                                                      where (rl.ListingOriginatorAppUserId == appUserId                                     && (rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial))
                                                                      select rl).ToList();

            return allRequirementsListingForUser;
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
                                                                            && (rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial))
                                                                            select rl).ToList();

            return allRequirementsListingForBranchUser;
        }

        #endregion

        #region Create

        public static RequirementListing CreateRequirementListing(IPrincipal user, string itemDescription, ItemTypeEnum itemType, decimal quantityRequired, string uom, DateTime? requiredFrom, DateTime? requiredTo, bool acceptDamagedItems, bool collectionAvailable, ItemRequiredListingStatusEnum listingStatus, Guid? selectedCampaignId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RequirementListing newRequirementListing = CreateRequirementListing(db, user, itemDescription, itemType, quantityRequired, uom, requiredFrom, requiredTo, acceptDamagedItems, collectionAvailable, listingStatus, selectedCampaignId);
            db.Dispose();
            return newRequirementListing;
        }

        public static RequirementListing CreateRequirementListing(ApplicationDbContext db, IPrincipal user, string itemDescription, ItemTypeEnum itemType, decimal quantityRequired, string uom, DateTime? requiredFrom, DateTime? requiredTo, bool acceptDamagedItems, bool collectionAvailable, ItemRequiredListingStatusEnum listingStatus, Guid? selectedCampaignId)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);
            Branch branch = BranchHelpers.GetBranch(db, branchUser.BranchId);

            RequirementListing requirementListing = new RequirementListing()
            {
                ListingId = Guid.NewGuid(),
                ItemDescription = itemDescription,
                ItemType = itemType,
                QuantityRequired = quantityRequired,
                QuantityFulfilled = 0,
                QuantityOutstanding = quantityRequired,
                UoM = uom,
                RequiredFrom = requiredFrom,
                RequiredTo = requiredTo,
                AcceptDamagedItems = acceptDamagedItems,
                CollectionAvailable = collectionAvailable,
                ListingBranchPostcode = branch.AddressPostcode,
                ListingOriginatorAppUserId = branchUser.UserId,
                ListingOriginatorBranchId = branchUser.BranchId,
                ListingOriginatorCompanyId = branchUser.CompanyId,
                ListingOriginatorDateTime = DateTime.Now,
                ListingStatus = ItemRequiredListingStatusEnum.Open,
                CampaignId = selectedCampaignId
            };

            db.RequirementListings.Add(requirementListing);
            db.SaveChanges();

            return requirementListing;
        }

        public static RequirementListing CreateRequirementListingFromRequirementListingAddView(RequirementListingAddView requirementListingAddView, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RequirementListing newCampaign = CreateRequirementListingFromRequirementListingAddView(db, requirementListingAddView, user);
            db.Dispose();
            return newCampaign;
        }

        public static RequirementListing CreateRequirementListingFromRequirementListingAddView(ApplicationDbContext db, RequirementListingAddView requirementListingAddView, IPrincipal user)
        {
            return CreateRequirementListing(db, user, requirementListingAddView.ItemDescription, requirementListingAddView.ItemType, requirementListingAddView.QuantityRequired, requirementListingAddView.UoM, requirementListingAddView.RequiredFrom, requirementListingAddView.RequiredTo, requirementListingAddView.AcceptDamagedItems, requirementListingAddView.CollectionAvailable, requirementListingAddView.ListingStatus, requirementListingAddView.SelectedCampaignId);
        }

        #endregion

        #region Update

        public static RequirementListing UpdateQuantitiesFromOrder(Offer offer)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RequirementListing listing = UpdateQuantitiesFromOrder(db, offer);
            db.Dispose();
            return listing;
        }

        public static RequirementListing UpdateQuantitiesFromOrder(ApplicationDbContext db, Offer offer)
        {
            RequirementListing listing = GetRequirementListing(db, offer.ListingId);
            listing.QuantityFulfilled += offer.CurrentOfferQuantity;
            listing.QuantityOutstanding -= offer.CurrentOfferQuantity;
            listing.ListingStatus = ItemRequiredListingStatusEnum.Partial;

            if (listing.QuantityOutstanding <= 0)
                listing.ListingStatus = ItemRequiredListingStatusEnum.Complete;

            db.Entry(listing).State = EntityState.Modified;
            db.SaveChanges();

            return listing;
        }

        #endregion
    }

    public static class RequirementListingGeneralInfoHelpers
    {
        #region Get

        public static List<RequirementListingGeneralInfoView> GetAllRequirementListingsGeneralInfoView(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<RequirementListingGeneralInfoView> list = GetAllRequirementListingsGeneralInfoView(db, user);
            db.Dispose();
            return list;
        }

        public static List<RequirementListingGeneralInfoView> GetAllRequirementListingsGeneralInfoView(ApplicationDbContext db, IPrincipal user)
        {
            List<RequirementListingGeneralInfoView> allRequirementListingsGeneralInfoView = new List<RequirementListingGeneralInfoView>();

            List<RequirementListing> allRequirementListings = RequirementListingHelpers.GetAllRequirementListings(db);

            foreach (RequirementListing requirementListing in allRequirementListings)
            {
                //Find any related offers
                Offer offer = OfferHelpers.GetOfferForListingAndUser(db, requirementListing.ListingId, AppUserHelpers.GetGuidFromUserGetAppUserId(user.Identity.GetAppUserId()));
                decimal offerQty = 0M;
                if (offer != null)
                    offerQty = offer.CurrentOfferQuantity;

                RequirementListingGeneralInfoView requirementListingGeneralInfoView = new RequirementListingGeneralInfoView()
                {
                    RequirementListing = requirementListing,
                    OfferQuantity = offerQty
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

        public static List<RequirementListingManageView> GetAllRequirementListingsManageViewForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<RequirementListingManageView> list = GetAllRequirementListingsManageViewForUser(db, user);
            db.Dispose();
            return list;
        }

        public static List<RequirementListingManageView> GetAllRequirementListingsManageViewForUser(ApplicationDbContext db, IPrincipal user)
        {
            List<RequirementListingManageView> allRequirementListingsManageView = new List<RequirementListingManageView>();

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            List<RequirementListing> allRequirementListingsForUser = RequirementListingHelpers.GetAllRequirementListingsForUser(db, appUser.AppUserId);

            foreach (RequirementListing requirementListingForBranchUser in allRequirementListingsForUser)
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