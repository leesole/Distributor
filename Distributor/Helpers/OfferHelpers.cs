using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.OfferEnums;

namespace Distributor.Helpers
{
    public static class OfferHelpers
    {
        #region Get

        public static Offer GetOffer(Guid offerId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Offer offer = GetOffer(db, offerId);
            db.Dispose();
            return offer;
        }

        public static Offer GetOffer(ApplicationDbContext db, Guid offerId)
        {
            return db.Offers.Find(offerId);
        }

        public static Offer GetOfferForListingAndUser(Guid listingId, Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Offer offer = GetOfferForListingAndUser(db, listingId, appUserId);
            db.Dispose();
            return offer;
        }

        public static Offer GetOfferForListingAndUser(ApplicationDbContext db, Guid listingId, Guid appUserId)
        {
            Offer offer = (from o in db.Offers
                           where (o.ListingId == listingId && o.OfferOriginatorAppUserId == appUserId)
                           select o).FirstOrDefault();


            return offer;
        }

        public static List<Offer> GetAllOffersForBranchUser(BranchUser branchUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Offer> list = GetAllOffersForBranchUser(db, branchUser);
            db.Dispose();
            return list;
        }

        public static List<Offer> GetAllOffersForBranchUser(ApplicationDbContext db, BranchUser branchUser)
        {
            List<Offer> allOffersForBranchUser = (from bu in db.BranchUsers
                                                  from o in db.Offers
                                                  where ((bu.UserId == o.OfferOriginatorAppUserId && bu.BranchId == o.OfferOriginatorBranchId && bu.CompanyId == o.OfferOriginatorCompanyId
                                                  && (o.OfferStatus == OfferStatusEnum.New || o.OfferStatus == OfferStatusEnum.Accepted))
                                                  || (bu.UserId == o.ListingOriginatorAppUserId && bu.BranchId == o.ListingOriginatorBranchId && bu.CompanyId == o.ListingOriginatorCompanyId))
                                                  select o).ToList();

            return allOffersForBranchUser;
        }

        #endregion

        #region Create

        public static Offer CreateOfferForRequirement(IPrincipal user, RequirementListing requirementListing, decimal offerQuantity)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Offer offer = CreateOfferForRequirement(db, user, requirementListing, offerQuantity);
            db.Dispose();
            return offer;
        }

        public static Offer CreateOfferForRequirement(ApplicationDbContext db, IPrincipal user, RequirementListing requirementListing, decimal offerQuantity)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);

            Offer offer = new Offer()
            {
                OfferId = Guid.NewGuid(),
                ListingId = requirementListing.ListingId,
                ListingType = ListingTypeEnum.Requirement,
                OfferStatus = OfferStatusEnum.New,
                CurrentOfferQuantity = offerQuantity,
                OfferOriginatorAppUserId = branchUser.UserId,
                OfferOriginatorBranchId = branchUser.BranchId,
                OfferOriginatorCompanyId = branchUser.CompanyId,
                OfferOriginatorDateTime = DateTime.Now,
                ListingOriginatorAppUserId = requirementListing.ListingOriginatorAppUserId,
                ListingOriginatorBranchId = requirementListing.ListingOriginatorBranchId,
                ListingOriginatorCompanyId = requirementListing.ListingOriginatorCompanyId,
                ListingOriginatorDateTime = requirementListing.ListingOriginatorDateTime
            };

            db.Offers.Add(offer);
            db.SaveChanges();

            return offer;
        }

        #endregion
    }

    public static class OfferManageHelpers
    {
        #region Get

        public static List<OfferManageView> GetAllOffersManageViewForUserBranch(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<OfferManageView> list = GetAllOffersManageViewForUserBranch(db, user);
            db.Dispose();
            return list;
        }

        public static List<OfferManageView> GetAllOffersManageViewForUserBranch(ApplicationDbContext db, IPrincipal user)
        {
            List<OfferManageView> allOffersManageView = new List<OfferManageView>();

            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);
            List<Offer> allOffersForBranchUser = OfferHelpers.GetAllOffersForBranchUser(db, branchUser);

            foreach (Offer offerForBranchUser in allOffersForBranchUser)
            {
                AvailableListing availableListing = null;
                RequirementListing requirementListing = null;

                switch (offerForBranchUser.ListingType)
                {
                    case ListingTypeEnum.Available:
                        availableListing = AvailableListingHelpers.GetAvailableListing(db, offerForBranchUser.ListingId);
                        break;
                    case ListingTypeEnum.Requirement:
                        requirementListing = RequirementListingHelpers.GetRequirementListing(db, offerForBranchUser.ListingId);
                        break;
                }

                OfferManageView offerManageView = new OfferManageView()
                {
                    OfferDetails = offerForBranchUser,
                    AvailableListingDetails = availableListing,
                    RequirementListingDetails = requirementListing,
                    AppUserDetails = AppUserHelpers.GetAppUser(db, branchUser.UserId),
                    BranchDetails = BranchHelpers.GetBranch(db, branchUser.BranchId)
                };

                allOffersManageView.Add(offerManageView);
            }

            return allOffersManageView;
        }

        #endregion
    }
}