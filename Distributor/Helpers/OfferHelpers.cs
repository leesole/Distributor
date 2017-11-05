using Distributor.Enums;
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
                           where (o.ListingId == listingId && o.OfferOriginatorAppUserId == appUserId && o.OfferStatus != OfferStatusEnum.Rejected)
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
                                    where ((o.OfferOriginatorAppUserId == appUserId && (o.OfferStatus == OfferStatusEnum.Accepted || o.OfferStatus == OfferStatusEnum.Rejected))
                                          || (o.ListingOriginatorAppUserId == appUserId && (o.OfferStatus == OfferStatusEnum.Accepted || o.OfferStatus == OfferStatusEnum.Rejected)))
                                    select o).Distinct().ToList();
            }
            else
            {
                allOffersForUser = (from o in db.Offers
                                    where ((o.OfferOriginatorAppUserId == appUserId && (o.OfferStatus == OfferStatusEnum.New || o.OfferStatus == OfferStatusEnum.Countered))
                                          || (o.ListingOriginatorAppUserId == appUserId && (o.OfferStatus == OfferStatusEnum.New || o.OfferStatus == OfferStatusEnum.Countered)))
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
                        where ((o.OfferOriginatorBranchId == branchId && (o.OfferStatus == OfferStatusEnum.Accepted || o.OfferStatus == OfferStatusEnum.Rejected))
                              || (o.ListingOriginatorBranchId == branchId && (o.OfferStatus == OfferStatusEnum.Accepted || o.OfferStatus == OfferStatusEnum.Rejected)))
                        select o).Distinct().ToList();
            }
            else
            {
                list = (from o in db.Offers
                        where ((o.OfferOriginatorBranchId == branchId && (o.OfferStatus == OfferStatusEnum.New || o.OfferStatus == OfferStatusEnum.Countered))
                              || (o.ListingOriginatorBranchId == branchId && (o.OfferStatus == OfferStatusEnum.New || o.OfferStatus == OfferStatusEnum.Countered)))
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
                        where ((o.OfferOriginatorCompanyId == companyId && (o.OfferStatus == OfferStatusEnum.Accepted || o.OfferStatus == OfferStatusEnum.Rejected))
                              || (o.ListingOriginatorCompanyId == companyId && (o.OfferStatus == OfferStatusEnum.Accepted || o.OfferStatus == OfferStatusEnum.Rejected)))
                        select o).Distinct().ToList();
            }
            else
            {
                list = (from o in db.Offers
                        where ((o.OfferOriginatorCompanyId == companyId && (o.OfferStatus == OfferStatusEnum.New || o.OfferStatus == OfferStatusEnum.Countered))
                              || (o.ListingOriginatorCompanyId == companyId && (o.OfferStatus == OfferStatusEnum.New || o.OfferStatus == OfferStatusEnum.Countered)))
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

            offer.OfferStatus = OfferStatusEnum.Countered;
            offer.CurrentOfferQuantity = offerQuantity;
            offer.CounterOfferQuantity = 0;
            offer.LastCounterOfferOriginatorAppUserId = branchUser.UserId;
            offer.LastCounterOfferOriginatorBranchId = branchUser.BranchId;
            offer.LastCounterOfferOriginatorCompanyId = branchUser.CompanyId;
            offer.LastCounterOfferOriginatorDateTime = DateTime.Now;

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
            offer.LastCounterOfferOriginatorAppUserId = branchUser.UserId;
            offer.LastCounterOfferOriginatorBranchId = branchUser.BranchId;
            offer.LastCounterOfferOriginatorCompanyId = branchUser.CompanyId;
            offer.LastCounterOfferOriginatorDateTime = DateTime.Now;

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

        public static OfferManageView GetOfferManageViewForOffer(Guid offerId, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            OfferManageView offerManageView = GetOfferManageViewForOffer(db, offerId, user);
            db.Dispose();
            return offerManageView;
        }

        public static OfferManageView GetOfferManageViewForOffer(ApplicationDbContext db, Guid offerId, IPrincipal user)
        {
            Offer offer = OfferHelpers.GetOffer(db, offerId);

            return GetOfferManageViewForOffer(db, offer, user);
        }

        public static OfferManageView GetOfferManageViewForOffer(Offer offer, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            OfferManageView offerManageView = GetOfferManageViewForOffer(db, offer, user);
            db.Dispose();
            return offerManageView;
        }

        public static OfferManageView GetOfferManageViewForOffer(ApplicationDbContext db, Offer offer, IPrincipal user)
        {
            if (offer.OfferStatus != OfferStatusEnum.New && offer.OfferStatus != OfferStatusEnum.Countered)
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

            AppUser thisAppUser = AppUserHelpers.GetAppUser(db, user);
            //If we allow branch trading then differentiate between branches for in/out trading, otherwise it is at company level
            Company thisCompany = CompanyHelpers.GetCompanyForUser(db, user);
            //set Inhouse flag
            offerManageView.InhouseOffer = OfferProcessHelpers.SetInhouseFlag(offer, thisAppUser, thisCompany);

            //set buttons
            bool? displayAcceptButton = null;
            bool? displayRejectButton = null;
            bool? displayCounterButton = null;
            bool? displayOfferButton = null;

            OfferProcessHelpers.SetOrderButtons(db, user, offerManageView, out displayAcceptButton, out displayRejectButton, out displayCounterButton, out displayOfferButton);

            offerManageView.DisplayAcceptButton = displayAcceptButton;
            offerManageView.DisplayRejectButton = displayRejectButton;
            offerManageView.DisplayCounterButton = displayCounterButton;
            offerManageView.DisplayOfferButton = displayOfferButton;

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
                allOffersManageView.Add(GetOfferManageViewForOffer(db, offerForUser, user));
            }

            return allOffersManageView;
        }

        #endregion
    }

    public static class OfferProcessHelpers
    {
        public static bool SetInhouseFlag(Offer offer, AppUser appUser, Company company)
        {
            if (company.AllowBranchTrading)
            {
                if (offer.OfferOriginatorBranchId == appUser.CurrentBranchId)
                    return true;
                else
                    return false;
            }
            else
            {
                if (offer.OfferOriginatorCompanyId == company.CompanyId)
                    return true;
                else
                    return false;
            }
        }

        public static void SetOrderButtons(ApplicationDbContext db, IPrincipal user, OfferManageView view, out bool? displayAcceptButton, out bool? displayRejectButton, out bool? displayCounterButton, out bool? displayOfferButton)
        {
            //Get settings for logged in user
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, user);

            Guid acceptedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OffersAcceptedAuthorisationManageViewLevel, user);
            Guid rejectedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OffersRejectedAuthorisationManageViewLevel, user);
            Guid returnedAuthorisationId = DataHelpers.GetAuthorisationId(settings.OffersReturnedAuthorisationManageViewLevel, user);

            //Set buttons
            displayAcceptButton = true;
            displayRejectButton = true;
            displayCounterButton = true;
            displayOfferButton = true;

            if (view.InhouseOffer)
            {
                switch (view.OfferAppUserSettings.OffersAcceptedAuthorisationManageViewLevel)
                {
                    case GeneralEnums.InternalSearchLevelEnum.Company:
                        if (view.OfferDetails.OfferOriginatorCompanyId != acceptedAuthorisationId)
                            displayAcceptButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Branch:
                        if (view.OfferDetails.OfferOriginatorBranchId != acceptedAuthorisationId)
                            displayAcceptButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.User:
                        if (view.OfferDetails.OfferOriginatorAppUserId != acceptedAuthorisationId)
                            displayAcceptButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Group:  //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }
                switch (view.OfferAppUserSettings.OffersRejectedAuthorisationManageViewLevel)
                {
                    case GeneralEnums.InternalSearchLevelEnum.Company:
                        if (view.OfferDetails.OfferOriginatorCompanyId != rejectedAuthorisationId)
                            displayRejectButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Branch:
                        if (view.OfferDetails.OfferOriginatorBranchId != rejectedAuthorisationId)
                            displayRejectButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.User:
                        if (view.OfferDetails.OfferOriginatorAppUserId != rejectedAuthorisationId)
                            displayRejectButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Group:  //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }
                switch (view.OfferAppUserSettings.OffersReturnedAuthorisationManageViewLevel)
                {
                    case GeneralEnums.InternalSearchLevelEnum.Company:
                        if (view.OfferDetails.OfferOriginatorCompanyId != returnedAuthorisationId)
                            displayOfferButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Branch:
                        if (view.OfferDetails.OfferOriginatorBranchId != returnedAuthorisationId)
                            displayOfferButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.User:
                        if (view.OfferDetails.OfferOriginatorAppUserId != returnedAuthorisationId)
                            displayOfferButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Group:  //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }
            }
            else
            {
                switch (view.ListingAppUserSettings.OffersAcceptedAuthorisationManageViewLevel)
                {
                    case GeneralEnums.InternalSearchLevelEnum.Company:
                        if (view.OfferDetails.ListingOriginatorCompanyId != acceptedAuthorisationId)
                            displayAcceptButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Branch:
                        if (view.OfferDetails.ListingOriginatorBranchId != acceptedAuthorisationId)
                            displayAcceptButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.User:
                        if (view.OfferDetails.ListingOriginatorAppUserId != acceptedAuthorisationId)
                            displayAcceptButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Group:  //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }
                switch (view.ListingAppUserSettings.OffersRejectedAuthorisationManageViewLevel)
                {
                    case GeneralEnums.InternalSearchLevelEnum.Company:
                        if (view.OfferDetails.ListingOriginatorCompanyId != rejectedAuthorisationId)
                            displayRejectButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Branch:
                        if (view.OfferDetails.ListingOriginatorBranchId != rejectedAuthorisationId)
                            displayRejectButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.User:
                        if (view.OfferDetails.ListingOriginatorAppUserId != rejectedAuthorisationId)
                            displayRejectButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Group:  //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }
                switch (view.ListingAppUserSettings.OffersReturnedAuthorisationManageViewLevel)
                {
                    case GeneralEnums.InternalSearchLevelEnum.Company:
                        if (view.OfferDetails.ListingOriginatorCompanyId != returnedAuthorisationId)
                            displayCounterButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Branch:
                        if (view.OfferDetails.ListingOriginatorBranchId != returnedAuthorisationId)
                            displayCounterButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.User:
                        if (view.OfferDetails.ListingOriginatorAppUserId != returnedAuthorisationId)
                            displayCounterButton = false;
                        break;
                    case GeneralEnums.InternalSearchLevelEnum.Group:  //LSLSLS  TO BE DONE WHEN GROUPS ADDED
                        break;
                }
            }
        }
    }
}