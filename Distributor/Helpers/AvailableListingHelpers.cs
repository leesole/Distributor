using Distributor.Extenstions;
using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.ItemEnums;

namespace Distributor.Helpers
{
    public static class AvailableListingHelpers
    {
        #region Get

        public static AvailableListing GetAvailableListing(Guid listingId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AvailableListing requirement = GetAvailableListing(db, listingId);
            db.Dispose();
            return requirement;
        }

        public static AvailableListing GetAvailableListing(ApplicationDbContext db, Guid listingId)
        {
            return db.AvailableListings.Find(listingId);
        }

        public static List<AvailableListing> GetAllAvailableListings()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListing> list = GetAllAvailableListings(db);
            db.Dispose();
            return list;
        }

        public static List<AvailableListing> GetAllAvailableListings(ApplicationDbContext db)
        {
            return db.AvailableListings.ToList();
        }
        
        public static List<AvailableListing> GetAllGeneralInfoFilteredAvailableListings(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListing> list = GetAllGeneralInfoFilteredAvailableListings(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<AvailableListing> GetAllGeneralInfoFilteredAvailableListings(ApplicationDbContext db, Guid appUserId)
        {

            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            var dateCheck = DateTime.MinValue;
            int settingsMaxDistance = settings.AvailableListingGeneralInfoMaxDistance ?? 0;

            //Create list within the time frame if setting set
            List<AvailableListing> list = (from rl in db.AvailableListings
                                           where ((rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial)
                                                   && rl.ListingOriginatorDateTime >= dateCheck)
                                           select rl).ToList();

            //Now bring in the Selection Level sort
            switch (settings.AvailableListingGeneralInfoExternalSelectionLevel)
            {
                case ExternalSearchLevelEnum.All: //do nothing
                    break;
                case ExternalSearchLevelEnum.Branch: //user's current branch to filter
                    list = list.Where(l => l.ListingOriginatorBranchId == branch.BranchId).ToList();
                    break;
                case ExternalSearchLevelEnum.Company: //user's current company to filter
                    list = list.Where(l => l.ListingOriginatorCompanyId == branch.CompanyId).ToList();
                    break;
                case ExternalSearchLevelEnum.Group: //user's built group sets to filter ***TO BE DONE***
                    break;
            }

            //Now sort by distance
            if (settingsMaxDistance > 0)
            {
                List<AvailableListing> removeList = new List<AvailableListing>();

                foreach (AvailableListing listing in list)
                {
                    DistanceHelpers distance = new DistanceHelpers();
                    int distanceValue = distance.GetDistance(branch.AddressPostcode, listing.ListingBranchPostcode);

                    if (distanceValue > settingsMaxDistance)
                        removeList.Add(listing);
                }

                if (removeList.Count > 0)
                    list.RemoveAll(l => removeList.Contains(l));
            }

            return list;
        }
                
        public static List<AvailableListing> GetAllManageListingFilteredAvailableListings(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListing> list = GetAllManageListingFilteredAvailableListings(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<AvailableListing> GetAllManageListingFilteredAvailableListings(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            var dateCheck = DateTime.MinValue;

            //Create list within the time frame if setting set
            List<AvailableListing> list = new List<AvailableListing>();

            //Now bring in the Selection Level sort
            switch (settings.AvailableListingManageViewInternalSelectionLevel)
            {
                case InternalSearchLevelEnum.User:
                    list = GetAllAvailableListingsForUser(db, appUserId);
                    break;
                case InternalSearchLevelEnum.Branch: //user's current branch to filter
                    list = GetAllAvailableListingsForBranch(db, branch.BranchId);
                    break;
                case InternalSearchLevelEnum.Company: //user's current company to filter
                    list = GetAllAvailableListingsForCompany(db, branch.CompanyId);
                    break;
                case InternalSearchLevelEnum.Group: //user's built group sets to filter ***TO BE DONE***
                    break;
            }

            return list;
        }

        public static List<AvailableListing> GetAllDashboardFilteredAvailableListings(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListing> list = GetAllDashboardFilteredAvailableListings(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<AvailableListing> GetAllDashboardFilteredAvailableListings(ApplicationDbContext db, Guid appUserId)
        {

            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            var dateCheck = DateTime.MinValue;
            double settingsMaxAge = settings.AvailableListingDashboardMaxAge ?? 0;
            int settingsMaxDistance = settings.AvailableListingDashboardMaxDistance ?? 0;

            if (settingsMaxAge > 0)
            {
                double negativeDays = 0 - settingsMaxAge;
                dateCheck = DateTime.Now.AddDays(negativeDays);
            }

            //Create list within the time frame if setting set
            List<AvailableListing> list = (from rl in db.AvailableListings
                                           where ((rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial)
                                                   && rl.ListingOriginatorDateTime >= dateCheck)
                                           select rl).ToList();

            //Now bring in the Selection Level sort
            switch (settings.AvailableListingDashboardExternalSelectionLevel)
            {
                case ExternalSearchLevelEnum.All: //do nothing
                    break;
                case ExternalSearchLevelEnum.Branch: //user's current branch to filter
                    list = list.Where(l => l.ListingOriginatorBranchId == branch.BranchId).ToList();
                    break;
                case ExternalSearchLevelEnum.Company: //user's current company to filter
                    list = list.Where(l => l.ListingOriginatorCompanyId == branch.CompanyId).ToList();
                    break;
                case ExternalSearchLevelEnum.Group: //user's built group sets to filter ***TO BE DONE***
                    break;
            }

            //Now sort by distance
            if (settingsMaxDistance > 0)
            {
                List<AvailableListing> removeList = new List<AvailableListing>();

                foreach (AvailableListing listing in list)
                {
                    DistanceHelpers distance = new DistanceHelpers();
                    int distanceValue = distance.GetDistance(branch.AddressPostcode, listing.ListingBranchPostcode);

                    if (distanceValue > settingsMaxDistance)
                        removeList.Add(listing);
                }

                if (removeList.Count > 0)
                    list.RemoveAll(l => removeList.Contains(l));
            }

            return list;
        }

        public static List<AvailableListing> GetAllAvailableListingsForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListing> list = GetAllAvailableListingsForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<AvailableListing> GetAllAvailableListingsForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<AvailableListing> allRequirementsListingForUser = (from rl in db.AvailableListings
                                                                     where (rl.ListingOriginatorAppUserId == appUserId && (rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial))
                                                                     select rl).ToList();

            return allRequirementsListingForUser;
        }

        public static List<AvailableListing> GetAllAvailableListingsForBranch(Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListing> list = GetAllAvailableListingsForBranch(db, branchId);
            db.Dispose();
            return list;
        }

        public static List<AvailableListing> GetAllAvailableListingsForBranch(ApplicationDbContext db, Guid branchId)
        {
            List<AvailableListing> allRequirementsListingForUser = (from rl in db.AvailableListings
                                                                    where (rl.ListingOriginatorBranchId == branchId && (rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial))
                                                                    select rl).ToList();

            return allRequirementsListingForUser;
        }

        public static List<AvailableListing> GetAllAvailableListingsForCompany(Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListing> list = GetAllAvailableListingsForCompany(db, companyId);
            db.Dispose();
            return list;
        }

        public static List<AvailableListing> GetAllAvailableListingsForCompany(ApplicationDbContext db, Guid companyId)
        {
            List<AvailableListing> allRequirementsListingForUser = (from rl in db.AvailableListings
                                                                    where (rl.ListingOriginatorCompanyId == companyId && (rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial))
                                                                    select rl).ToList();

            return allRequirementsListingForUser;
        }

        public static List<AvailableListing> GetAllAvailableListingsForBranchUser(BranchUser branchUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListing> list = GetAllAvailableListingsForBranchUser(db, branchUser);
            db.Dispose();
            return list;
        }

        public static List<AvailableListing> GetAllAvailableListingsForBranchUser(ApplicationDbContext db, BranchUser branchUser)
        {
            List<AvailableListing> allRequirementsListingForBranchUser = (from bu in db.BranchUsers
                                                                            from rl in db.AvailableListings
                                                                            where (bu.UserId == rl.ListingOriginatorAppUserId && bu.BranchId == rl.ListingOriginatorBranchId && bu.CompanyId == rl.ListingOriginatorCompanyId
                                                                            && (rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial))
                                                                            select rl).ToList();

            return allRequirementsListingForBranchUser;
        }

        #endregion

        #region Create

        public static AvailableListing CreateAvailableListing(IPrincipal user, string itemDescription, ItemTypeEnum itemType, decimal quantityRequired, string uom, DateTime? availableFrom, DateTime? availableTo, ItemConditionEnum itemCondition, DateTime? displayUntilDate, DateTime? sellByDate, DateTime? useByDate, bool deliveryAvailable, ItemRequiredListingStatusEnum listingStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AvailableListing newAvailableListing = CreateAvailableListing(db, user, itemDescription, itemType, quantityRequired, uom, availableFrom, availableTo, itemCondition, displayUntilDate, sellByDate, useByDate, deliveryAvailable, listingStatus);
            db.Dispose();
            return newAvailableListing;
        }

        public static AvailableListing CreateAvailableListing(ApplicationDbContext db, IPrincipal user, string itemDescription, ItemTypeEnum itemType, decimal quantityRequired, string uom, DateTime? availableFrom, DateTime? availableTo, ItemConditionEnum itemCondition, DateTime? displayUntilDate, DateTime? sellByDate, DateTime? useByDate, bool deliveryAvailable, ItemRequiredListingStatusEnum listingStatus)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);
            Branch branch = BranchHelpers.GetBranch(db, branchUser.BranchId);

            AvailableListing AvailableListing = new AvailableListing()
            {
                ListingId = Guid.NewGuid(),
                ItemDescription = itemDescription,
                ItemType = itemType,
                QuantityRequired = quantityRequired,
                QuantityFulfilled = 0,
                QuantityOutstanding = quantityRequired,
                UoM = uom,
                AvailableFrom = availableFrom,
                AvailableTo = availableTo,
                ItemCondition = itemCondition,
                DisplayUntilDate = displayUntilDate,
                SellByDate = sellByDate,
                UseByDate = useByDate,
                DeliveryAvailable = deliveryAvailable,
                ListingBranchPostcode = branch.AddressPostcode,
                ListingOriginatorAppUserId = branchUser.UserId,
                ListingOriginatorBranchId = branchUser.BranchId,
                ListingOriginatorCompanyId = branchUser.CompanyId,
                ListingOriginatorDateTime = DateTime.Now,
                ListingStatus = ItemRequiredListingStatusEnum.Open
            };

            db.AvailableListings.Add(AvailableListing);
            db.SaveChanges();

            return AvailableListing;
        }

        public static AvailableListing CreateAvailableListingFromAvailableListingAddView(AvailableListingAddView AvailableListingAddView, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AvailableListing newCampaign = CreateAvailableListingFromAvailableListingAddView(db, AvailableListingAddView, user);
            db.Dispose();
            return newCampaign;
        }

        public static AvailableListing CreateAvailableListingFromAvailableListingAddView(ApplicationDbContext db, AvailableListingAddView AvailableListingAddView, IPrincipal user)
        {
            return CreateAvailableListing(db, user, AvailableListingAddView.ItemDescription, AvailableListingAddView.ItemType, AvailableListingAddView.QuantityRequired, AvailableListingAddView.UoM, AvailableListingAddView.AvailableFrom, AvailableListingAddView.AvailableTo, AvailableListingAddView.ItemCondition, AvailableListingAddView.DisplayUntilDate, AvailableListingAddView.SellByDate, AvailableListingAddView.UseByDate, AvailableListingAddView.DeliveryAvailable, AvailableListingAddView.ListingStatus);
        }

        #endregion

        #region Update

        public static AvailableListing UpdateQuantitiesFromOrder(Offer offer)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AvailableListing listing = UpdateQuantitiesFromOrder(db, offer);
            db.Dispose();
            return listing;
        }

        public static AvailableListing UpdateQuantitiesFromOrder(ApplicationDbContext db, Offer offer)
        {
            AvailableListing listing = GetAvailableListing(db, offer.ListingId);
            listing.QuantityFulfilled += offer.CurrentOfferQuantity;
            listing.QuantityOutstanding -= offer.CurrentOfferQuantity;
            listing.ListingStatus = ItemRequiredListingStatusEnum.Partial;

            if (listing.QuantityOutstanding <= 0)
                listing.ListingStatus = ItemRequiredListingStatusEnum.Complete;

            db.Entry(listing).State = EntityState.Modified;
            db.SaveChanges();

            return listing;
        }

        public static AvailableListing UpdateAvailableListingFromAvailableListingEditView(AvailableListingEditView view)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AvailableListing listing = UpdateAvailableListingFromAvailableListingEditView(db, view);
            db.Dispose();
            return listing;
        }

        public static AvailableListing UpdateAvailableListingFromAvailableListingEditView(ApplicationDbContext db, AvailableListingEditView view)
        {
            AvailableListing listing = GetAvailableListing(db, view.ListingId);
            listing.ItemDescription = view.ItemDescription;
            listing.ItemType = view.ItemType;
            listing.QuantityRequired = view.QuantityRequired;
            listing.QuantityFulfilled = view.QuantityFulfilled;
            listing.QuantityOutstanding = view.QuantityOutstanding;
            listing.UoM = view.UoM;
            listing.AvailableFrom = view.AvailableFrom;
            listing.AvailableTo = view.AvailableTo;
            listing.ItemCondition = view.ItemCondition;
            listing.DisplayUntilDate = view.DisplayUntilDate;
            listing.SellByDate = view.SellByDate;
            listing.UseByDate = view.UseByDate;
            listing.DeliveryAvailable = view.DeliveryAvailable;
            listing.ListingStatus = view.ListingStatus;

            db.Entry(listing).State = EntityState.Modified;
            db.SaveChanges();

            return listing;
        }

        #endregion
    }

    public static class AvailableListingGeneralInfoHelpers
    {
        #region Get

        public static List<AvailableListingGeneralInfoView> GetAllAvailableListingsGeneralInfoView(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListingGeneralInfoView> list = GetAllAvailableListingsGeneralInfoView(db, user);
            db.Dispose();
            return list;
        }

        public static List<AvailableListingGeneralInfoView> GetAllAvailableListingsGeneralInfoView(ApplicationDbContext db, IPrincipal user)
        {
            List<AvailableListingGeneralInfoView> allAvailableListingsGeneralInfoView = new List<AvailableListingGeneralInfoView>();

            List<AvailableListing> allAvailableListings = AvailableListingHelpers.GetAllGeneralInfoFilteredAvailableListings(db, AppUserHelpers.GetAppUserIdFromUser(user));

            foreach (AvailableListing AvailableListing in allAvailableListings)
            {
                //Find any related offers
                Offer offer = OfferHelpers.GetOfferForListingAndUser(db, AvailableListing.ListingId, AppUserHelpers.GetGuidFromUserGetAppUserId(user.Identity.GetAppUserId()));
                decimal offerQty = 0M;
                if (offer != null)
                    offerQty = offer.CurrentOfferQuantity;

                AvailableListingGeneralInfoView AvailableListingGeneralInfoView = new AvailableListingGeneralInfoView()
                {
                    AvailableListing = AvailableListing,
                    OfferQuantity = offerQty
                };

                allAvailableListingsGeneralInfoView.Add(AvailableListingGeneralInfoView);
            }

            return allAvailableListingsGeneralInfoView;
        }

        #endregion
    }

    public static class AvailableListingManageHelpers
    {
        #region Get

        public static List<AvailableListingManageView> GetAllAvailableListingsManageView(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListingManageView> list = GetAllAvailableListingsManageView(db, user);
            db.Dispose();
            return list;
        }

        public static List<AvailableListingManageView> GetAllAvailableListingsManageView(ApplicationDbContext db, IPrincipal user)
        {
            List<AvailableListingManageView> allAvailableListingsManageView = new List<AvailableListingManageView>();

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            List<AvailableListing> allAvailableListingsForUser = AvailableListingHelpers.GetAllManageListingFilteredAvailableListings(db, appUser.AppUserId);

            foreach (AvailableListing AvailableListingForBranchUser in allAvailableListingsForUser)
            {
                AvailableListingManageView AvailableListingManageView = new AvailableListingManageView()
                {
                    AvailableListing = AvailableListingForBranchUser
                };

                allAvailableListingsManageView.Add(AvailableListingManageView);
            }

            return allAvailableListingsManageView;
        }

        public static List<AvailableListingManageView> GetAllAvailableListingsManageViewForUserBranch(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListingManageView> list = GetAllAvailableListingsManageViewForUserBranch(db, user);
            db.Dispose();
            return list;
        }

        public static List<AvailableListingManageView> GetAllAvailableListingsManageViewForUserBranch(ApplicationDbContext db, IPrincipal user)
        {
            List<AvailableListingManageView> allAvailableListingsManageView = new List<AvailableListingManageView>();

            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);
            List<AvailableListing> allAvailableListingsForBranchUser = AvailableListingHelpers.GetAllAvailableListingsForBranchUser(db, branchUser);

            foreach (AvailableListing AvailableListingForBranchUser in allAvailableListingsForBranchUser)
            {
                AvailableListingManageView AvailableListingManageView = new AvailableListingManageView()
                {
                    AvailableListing = AvailableListingForBranchUser
                };

                allAvailableListingsManageView.Add(AvailableListingManageView);
            }

            return allAvailableListingsManageView;
        }

        #endregion
    }

    public static class AvailableListingEditHelpers
    {
        #region Get

        public static AvailableListingEditView GetAvailableListingEditView(Guid listingId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            AvailableListingEditView view = GetAvailableListingEditView(db, listingId);
            db.Dispose();
            return view;
        }

        public static AvailableListingEditView GetAvailableListingEditView(ApplicationDbContext db, Guid listingId)
        {
            AvailableListing availableListing = AvailableListingHelpers.GetAvailableListing(db, listingId);
            AppUser listingAppUser = AppUserHelpers.GetAppUser(db, availableListing.ListingOriginatorAppUserId);
            Branch listingBranch = BranchHelpers.GetBranch(db, availableListing.ListingOriginatorBranchId);

            AvailableListingEditView view = new AvailableListingEditView()
            {
                ListingId = availableListing.ListingId,
                ItemDescription = availableListing.ItemDescription,
                ItemType = availableListing.ItemType,
                QuantityRequired = availableListing.QuantityRequired,
                QuantityFulfilled = availableListing.QuantityFulfilled,
                QuantityOutstanding = availableListing.QuantityOutstanding,
                UoM = availableListing.UoM,
                AvailableFrom = availableListing.AvailableFrom,
                AvailableTo = availableListing.AvailableTo,
                ItemCondition = availableListing.ItemCondition,
                DisplayUntilDate = availableListing.DisplayUntilDate,
                SellByDate = availableListing.SellByDate,
                UseByDate = availableListing.UseByDate,
                DeliveryAvailable = availableListing.DeliveryAvailable,
                ListingStatus = availableListing.ListingStatus,
                ListingOriginatorDateTime = availableListing.ListingOriginatorDateTime,
                ListingAppUser = listingAppUser,
                ListingBranchDetails = listingBranch
            };

            return view;
        }

        #endregion
    }
}