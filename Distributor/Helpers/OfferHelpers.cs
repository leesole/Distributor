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

        public static List<Offer> GetAllOffersForUser(Guid appUserId, bool getHistory)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Offer> list = GetAllOffersForUser(db, appUserId, getHistory);
            db.Dispose();
            return list;
        }

        public static List<Offer> GetAllOffersForUser(ApplicationDbContext db, Guid appUserId, bool getHistory)
        {
            List<Offer> allOffersForUser = null;

            if (getHistory)
            {
                allOffersForUser = (from o in db.Offers
                                    where ((o.OfferOriginatorAppUserId == appUserId && o.OfferStatus != OfferStatusEnum.New)
                                          || (o.ListingOriginatorAppUserId == appUserId && o.OfferStatus != OfferStatusEnum.New))
                                    select o).Distinct().ToList();
            }
            else
            {
                allOffersForUser = (from o in db.Offers
                                    where ((o.OfferOriginatorAppUserId == appUserId && o.OfferStatus == OfferStatusEnum.New)
                                          || (o.ListingOriginatorAppUserId == appUserId && o.OfferStatus == OfferStatusEnum.New))
                                    select o).Distinct().ToList();
            }

            return allOffersForUser;
        }

        public static List<Offer> GetAllOffersForBranch(Guid branchId, bool getHistory)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Offer> list = GetAllOffersForBranch(db, branchId, getHistory);
            db.Dispose();
            return list;
        }

        public static List<Offer> GetAllOffersForBranch(ApplicationDbContext db, Guid branchId, bool getHistory)
        {
            List<Offer> list = null;

            if (getHistory)
            {
                list = (from o in db.Offers
                        where ((o.OfferOriginatorBranchId == branchId && o.OfferStatus != OfferStatusEnum.New)
                              || (o.ListingOriginatorBranchId == branchId && o.OfferStatus != OfferStatusEnum.New))
                        select o).Distinct().ToList();
            }
            else
            {
                list = (from o in db.Offers
                        where ((o.OfferOriginatorBranchId == branchId && o.OfferStatus == OfferStatusEnum.New)
                              || (o.ListingOriginatorBranchId == branchId && o.OfferStatus == OfferStatusEnum.New))
                        select o).Distinct().ToList();
            }

            return list;
        }

        public static List<Offer> GetAllOffersForCompany(Guid companyId, bool getHistory)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Offer> list = GetAllOffersForCompany(db, companyId, getHistory);
            db.Dispose();
            return list;
        }

        public static List<Offer> GetAllOffersForCompany(ApplicationDbContext db, Guid companyId, bool getHistory)
        {
            List<Offer> list = null;

            if (getHistory)
            {
                list = (from o in db.Offers
                        where ((o.OfferOriginatorCompanyId == companyId && o.OfferStatus != OfferStatusEnum.New)
                              || (o.ListingOriginatorCompanyId == companyId && o.OfferStatus != OfferStatusEnum.New))
                        select o).Distinct().ToList();
            }
            else
            {
                list = (from o in db.Offers
                        where ((o.OfferOriginatorCompanyId == companyId && o.OfferStatus == OfferStatusEnum.New)
                              || (o.ListingOriginatorCompanyId == companyId && o.OfferStatus == OfferStatusEnum.New))
                        select o).Distinct().ToList();
            }

            return list;
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

        public static List<Offer> GetAllManageListingFilteredOffers(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Offer> list = GetAllManageListingFilteredOffers(db, appUserId, false);
            db.Dispose();
            return list;
        }

        public static List<Offer> GetAllManageListingFilteredOffers(Guid appUserId, bool getHistory)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Offer> list = GetAllManageListingFilteredOffers(db, appUserId, getHistory);
            db.Dispose();
            return list;
        }

        public static List<Offer> GetAllManageListingFilteredOffers(ApplicationDbContext db, Guid appUserId, bool getHistory)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            //Create list 
            List<Offer> list = new List<Offer>();

            //Now bring in the Selection Level sort
            switch (settings.OffersManageViewInternalSelectionLevel)
            {
                case InternalSearchLevelEnum.User:
                    list = GetAllOffersForUser(db, appUserId, getHistory);
                    break;
                case InternalSearchLevelEnum.Branch: //user's current branch to filter
                    list = GetAllOffersForBranch(db, branch.BranchId, getHistory);
                    break;
                case InternalSearchLevelEnum.Company: //user's current company to filter
                    list = GetAllOffersForCompany(db, branch.CompanyId, getHistory);
                    break;
                case InternalSearchLevelEnum.Group: //user's built group sets to filter ***TO BE DONE***
                    break;
            }

            return list;
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

        public static OfferManageView GetOfferManageViewForOffer(Guid offerId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            OfferManageView offerManageView = GetOfferManageViewForOffer(db, offerId);
            db.Dispose();
            return offerManageView;
        }

        public static OfferManageView GetOfferManageViewForOffer(ApplicationDbContext db, Guid offerId)
        {
            Offer offer = OfferHelpers.GetOffer(db, offerId);

            return GetOfferManageViewForOffer(db, offer);
        }

        public static OfferManageView GetOfferManageViewForOffer(Offer offer)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            OfferManageView offerManageView = GetOfferManageViewForOffer(db, offer);
            db.Dispose();
            return offerManageView;
        }

        public static OfferManageView GetOfferManageViewForOffer(ApplicationDbContext db, Offer offer)
        {
            if (offer.OfferStatus != OfferStatusEnum.New)
                return null;

            AppUser offerAppUser = AppUserHelpers.GetAppUser(db, offer.OfferOriginatorAppUserId);

            AvailableListing availableListing = null;
            RequirementListing requirementListing = null;
            AppUser listingAppUser = null;

            switch (offer.ListingType)
            {
                case ListingTypeEnum.Available:
                    availableListing = AvailableListingHelpers.GetAvailableListing(db, offer.ListingId);
                    listingAppUser = AppUserHelpers.GetAppUser(db, availableListing.ListingOriginatorAppUserId);
                    break;
                case ListingTypeEnum.Requirement:
                    requirementListing = RequirementListingHelpers.GetRequirementListing(db, offer.ListingId);
                    listingAppUser = AppUserHelpers.GetAppUser(db, requirementListing.ListingOriginatorAppUserId);
                    break;
            }

            OfferManageView offerManageView = new OfferManageView()
            {
                OfferDetails = offer,
                AvailableListingDetails = availableListing,
                RequirementListingDetails = requirementListing,
                OfferAppUserDetails = offerAppUser,
                ListingAppUserDetails = listingAppUser,
                OfferBranchDetails = BranchHelpers.GetBranch(db, offerAppUser.CurrentBranchId),
                ListingBranchDetails = BranchHelpers.GetBranch(db, listingAppUser.CurrentBranchId),
                OfferAppUserSettings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, offerAppUser.AppUserId),
                ListingAppUserSettings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, listingAppUser.AppUserId)
        };

            return offerManageView;
        }

        public static List<OfferManageView> GetAllOffersManageView(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<OfferManageView> list = GetAllOffersManageView(db, user, false);
            db.Dispose();
            return list;
        }

        public static List<OfferManageView> GetAllOffersManageView(IPrincipal user, bool getHistory)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<OfferManageView> list = GetAllOffersManageView(db, user, getHistory);
            db.Dispose();
            return list;
        }

        public static List<OfferManageView> GetAllOffersManageView(ApplicationDbContext db, IPrincipal user, bool getHistory)
        {
            List<OfferManageView> allOffersManageView = new List<OfferManageView>();

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            List<Offer> allOffersForUser = OfferHelpers.GetAllManageListingFilteredOffers(db, appUser.AppUserId, getHistory);

            foreach (Offer offerForUser in allOffersForUser)
            {
                //AvailableListing availableListing = null;
                //RequirementListing requirementListing = null;
                //AppUser listingAppUser = null;

                //AppUser offerAppUser = AppUserHelpers.GetAppUser(db, offerForUser.OfferOriginatorAppUserId);

                //switch (offerForUser.ListingType)
                //{
                //    case ListingTypeEnum.Available:
                //        availableListing = AvailableListingHelpers.GetAvailableListing(db, offerForUser.ListingId);
                //        listingAppUser = AppUserHelpers.GetAppUser(db, availableListing.ListingOriginatorAppUserId);
                //        break;
                //    case ListingTypeEnum.Requirement:
                //        requirementListing = RequirementListingHelpers.GetRequirementListing(db, offerForUser.ListingId);
                //        listingAppUser = AppUserHelpers.GetAppUser(db, requirementListing.ListingOriginatorAppUserId);
                //        break;
                //}

                //OfferManageView offerManageView = new OfferManageView()
                //{
                //    OfferDetails = offerForUser,
                //    AvailableListingDetails = availableListing,
                //    RequirementListingDetails = requirementListing,
                //    OfferAppUserDetails = offerAppUser,
                //    ListingAppUserDetails = listingAppUser,
                //    OfferBranchDetails = BranchHelpers.GetBranch(db, offerAppUser.CurrentBranchId),
                //    ListingBranchDetails = BranchHelpers.GetBranch(db, listingAppUser.CurrentBranchId)
                //};

                allOffersManageView.Add(GetOfferManageViewForOffer(db, offerForUser));
            }

            return allOffersManageView;
        }

        #endregion
    }
}