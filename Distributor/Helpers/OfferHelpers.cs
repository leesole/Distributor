using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public static List<Offer> GetAllOffersForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Offer> list = GetAllOffersForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Offer> GetAllOffersForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<Offer> allOffersForUser = (from o in db.Offers
                                            where ((o.OfferOriginatorAppUserId == appUserId && o.OfferStatus == OfferStatusEnum.New)
                                                  || (o.ListingOriginatorAppUserId == appUserId && o.OfferStatus == OfferStatusEnum.New))
                                            select o).ToList();
            var allOffersForUserDistinct = allOffersForUser.Distinct().ToList();

            return allOffersForUserDistinct;
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
            var allOffersForBranchUserDistinct = allOffersForBranchUser.Distinct().ToList();

            return allOffersForBranchUserDistinct;
        }

        #endregion

        #region Create

        public static Offer CreateOfferForAvailable(IPrincipal user, AvailableListing availableListing, decimal offerQuantity)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Offer offer = CreateOfferForAvailable(db, user, availableListing, offerQuantity);
            db.Dispose();
            return offer;
        }

        public static Offer CreateOfferForAvailable(ApplicationDbContext db, IPrincipal user, AvailableListing availableListing, decimal offerQuantity)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);

            Offer offer = new Offer()
            {
                OfferId = Guid.NewGuid(),
                ListingId = availableListing.ListingId,
                ListingType = ListingTypeEnum.Available,
                OfferStatus = OfferStatusEnum.New,
                CurrentOfferQuantity = offerQuantity,
                OfferOriginatorAppUserId = branchUser.UserId,
                OfferOriginatorBranchId = branchUser.BranchId,
                OfferOriginatorCompanyId = branchUser.CompanyId,
                OfferOriginatorDateTime = DateTime.Now,
                ListingOriginatorAppUserId = availableListing.ListingOriginatorAppUserId,
                ListingOriginatorBranchId = availableListing.ListingOriginatorBranchId,
                ListingOriginatorCompanyId = availableListing.ListingOriginatorCompanyId,
                ListingOriginatorDateTime = availableListing.ListingOriginatorDateTime
            };

            db.Offers.Add(offer);
            db.SaveChanges();

            return offer;
        }

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

        #region Update

        public static Offer UpdateOffer(IPrincipal user, Offer offer, decimal offerQuantity)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Offer updatedOffer = UpdateOffer(db, user, offer, offerQuantity);
            db.Dispose();
            return updatedOffer;
        }

        public static Offer UpdateOffer(ApplicationDbContext db, IPrincipal user, Offer offer, decimal offerQuantity)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);

            offer.CurrentOfferQuantity = offerQuantity;
            offer.CounterOfferQuantity = 0;

            db.Entry(offer).State = EntityState.Modified;
            db.SaveChanges();

            return offer;
        }

        public static Offer UpdateCounterOffer(IPrincipal user, Offer offer, decimal offerQuantity)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Offer updatedOffer = UpdateCounterOffer(db, user, offer, offerQuantity);
            db.Dispose();
            return updatedOffer;
        }

        public static Offer UpdateCounterOffer(ApplicationDbContext db, IPrincipal user, Offer offer, decimal offerQuantity)
        { 
            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);

            offer.CounterOfferQuantity = offerQuantity;
            offer.PreviousOfferQuantity = offer.CurrentOfferQuantity;
            offer.CurrentOfferQuantity = 0;

            db.Entry(offer).State = EntityState.Modified;
            db.SaveChanges();

            return offer;
        }

        public static Offer RejectOffer(IPrincipal user, Offer offer)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Offer rejectedOffer = RejectOffer(db, user, offer);
            db.Dispose();
            return rejectedOffer;
        }

        public static Offer RejectOffer(ApplicationDbContext db, IPrincipal user, Offer offer)
        {
            offer.OfferStatus = OfferStatusEnum.Rejected;
            offer.RejectedBy = AppUserHelpers.GetAppUserIdFromUser(user);
            offer.RejectedOn = DateTime.Now;

            db.Entry(offer).State = EntityState.Modified;
            db.SaveChanges();

            return offer;
        }

        public static Offer AcceptOffer(IPrincipal user, Offer offer)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Offer acceptedOffer = AcceptOffer(db, user, offer);
            db.Dispose();
            return acceptedOffer;
        }

        public static Offer AcceptOffer(ApplicationDbContext db, IPrincipal user, Offer offer)
        {
            offer.OfferStatus = OfferStatusEnum.Accepted;

            Order order = OrderHelpers.CreateOrder(db, user, offer);

            offer.OrderId = order.OrderId;
            offer.OrderOriginatorAppUserId = order.OrderOriginatorAppUserId;
            offer.OrderOriginatorBranchId = order.OrderOriginatorBranchId;
            offer.OrderOriginatorCompanyId = order.OrderOriginatorCompanyId;
            offer.OrderOriginatorDateTime = order.OrderOriginatorDateTime;

            db.Entry(offer).State = EntityState.Modified;
            db.SaveChanges();

            return offer;
        }

        #endregion
    }

    public static class OfferManageHelpers
    {
        #region Get

        public static List<OfferManageView> GetAllOffersManageViewForUser(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<OfferManageView> list = GetAllOffersManageViewForUser(db, user);
            db.Dispose();
            return list;
        }

        public static List<OfferManageView> GetAllOffersManageViewForUser(ApplicationDbContext db, IPrincipal user)
        {
            List<OfferManageView> allOffersManageView = new List<OfferManageView>();

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            List<Offer> allOffersForUser = OfferHelpers.GetAllOffersForUser(db, appUser.AppUserId);

            foreach (Offer offerForUser in allOffersForUser)
            {
                AvailableListing availableListing = null;
                RequirementListing requirementListing = null;

                switch (offerForUser.ListingType)
                {
                    case ListingTypeEnum.Available:
                        availableListing = AvailableListingHelpers.GetAvailableListing(db, offerForUser.ListingId);
                        break;
                    case ListingTypeEnum.Requirement:
                        requirementListing = RequirementListingHelpers.GetRequirementListing(db, offerForUser.ListingId);
                        break;
                }

                OfferManageView offerManageView = new OfferManageView()
                {
                    OfferDetails = offerForUser,
                    AvailableListingDetails = availableListing,
                    RequirementListingDetails = requirementListing,
                    AppUserDetails = appUser,
                    BranchDetails = BranchHelpers.GetBranch(db, appUser.CurrentBranchId)
                };

                allOffersManageView.Add(offerManageView);
            }

            return allOffersManageView;
        }

        #endregion
    }
}