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

        public static List<RequirementListing> GetAllGeneralInfoFilteredRequirementListings(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<RequirementListing> list = GetAllGeneralInfoFilteredRequirementListings(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<RequirementListing> GetAllGeneralInfoFilteredRequirementListings(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            var dateCheck = DateTime.MinValue;
            int settingsMaxDistance = settings.RequiredListingGeneralInfoMaxDistance ?? 0;

            //Create list within the time frame if setting set
            List<RequirementListing> list = (from rl in db.RequirementListings
                                             where ((rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial)
                                             && rl.ListingOriginatorDateTime >= dateCheck)
                                             orderby rl.RequiredTo ?? DateTime.MaxValue
                                             select rl).ToList();

            //Now bring in the Selection Level sort
            switch (settings.RequiredListingDashboardExternalSelectionLevel)
            {
                case ExternalSearchLevelEnum.All: //do nothing
                    break;
                case ExternalSearchLevelEnum.Branch: //user's current branch to filter
                    list = list.Where(l => l.ListingOriginatorBranchId == branch.BranchId).ToList();
                    break;
                case ExternalSearchLevelEnum.Company: //user's current company to filter
                    list = list.Where(l => l.ListingOriginatorCompanyId == branch.CompanyId).ToList();
                    break;
                case ExternalSearchLevelEnum.Group: //LSLSLSuser's built group sets to filter ***TO BE DONE***
                    break;
            }

            //Now sort by distance
            if (settingsMaxDistance > 0)
            {
                List<RequirementListing> removeList = new List<RequirementListing>();

                foreach (RequirementListing listing in list)
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

        public static List<RequirementListing> GetAllManageListingFilteredRequirementListings(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<RequirementListing> list = GetAllManageListingFilteredRequirementListings(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<RequirementListing> GetAllManageListingFilteredRequirementListings(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            //Create list
            List<RequirementListing> list = new List<RequirementListing>();

            //Now bring in the Selection Level sort
            switch (settings.RequiredListingManageViewInternalSelectionLevel)
            {
                case InternalSearchLevelEnum.User:
                    list = GetAllRequirementListingsForUser(db, appUserId);
                    break;
                case InternalSearchLevelEnum.Branch: //user's current branch to filter
                    list = GetAllRequirementListingsForBranch(db, branch.BranchId);
                    break;
                case InternalSearchLevelEnum.Company: //user's current company to filter
                    list = GetAllRequirementListingsForCompany(db, branch.CompanyId);
                    break;
                case InternalSearchLevelEnum.Group: //user's built group sets to filter ***TO BE DONE***
                    break;
            }

            return list;
        }

        public static List<RequirementListing> GetAllDashboardFilteredRequirementListings(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<RequirementListing> list = GetAllDashboardFilteredRequirementListings(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<RequirementListing> GetAllDashboardFilteredRequirementListings(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            var dateCheck = DateTime.MinValue;
            double settingsMaxAge = settings.RequiredListingDashboardMaxAge ?? 0;
            int settingsMaxDistance = settings.RequiredListingDashboardMaxDistance ?? 0;

            if (settingsMaxAge > 0)
            {
                double negativeDays = 0 - settingsMaxAge;
                dateCheck = DateTime.Now.AddDays(negativeDays);
            }

            //Create list within the time frame if setting set
            List<RequirementListing> list = (from rl in db.RequirementListings
                                             where ((rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial)
                                             && rl.ListingOriginatorDateTime >= dateCheck)
                                             select rl).ToList();

            //Now bring in the Selection Level sort
            switch (settings.RequiredListingDashboardExternalSelectionLevel)
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
                List<RequirementListing> removeList = new List<RequirementListing>();

                foreach (RequirementListing listing in list)
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
                                                                      where (rl.ListingOriginatorAppUserId == appUserId && (rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial))
                                                                      orderby rl.RequiredTo ?? DateTime.MaxValue
                                                                      select rl).ToList();

            return allRequirementsListingForUser;
        }

        public static List<RequirementListing> GetAllRequirementListingsForBranch(Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<RequirementListing> list = GetAllRequirementListingsForBranch(db, branchId);
            db.Dispose();
            return list;
        }

        public static List<RequirementListing> GetAllRequirementListingsForBranch(ApplicationDbContext db, Guid branchId)
        {
            List<RequirementListing> list = (from rl in db.RequirementListings
                                             where (rl.ListingOriginatorBranchId == branchId && (rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial))
                                             orderby rl.RequiredTo ?? DateTime.MaxValue
                                             select rl).ToList();

            return list;
        }

        public static List<RequirementListing> GetAllRequirementListingsForCompany(Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<RequirementListing> list = GetAllRequirementListingsForCompany(db, companyId);
            db.Dispose();
            return list;
        }

        public static List<RequirementListing> GetAllRequirementListingsForCompany(ApplicationDbContext db, Guid companyId)
        {
            List<RequirementListing> list = (from rl in db.RequirementListings
                                             where (rl.ListingOriginatorCompanyId == companyId && (rl.ListingStatus == ItemRequiredListingStatusEnum.Open || rl.ListingStatus == ItemRequiredListingStatusEnum.Partial))
                                             orderby rl.RequiredTo ?? DateTime.MaxValue
                                             select rl).ToList();

            return list;
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

        public static RequirementListing CreateRequirementListing(IPrincipal user, string itemDescription, ItemTypeEnum itemType, decimal quantityRequired, string uom, DateTime? requiredFrom, DateTime? requiredTo, bool acceptDamagedItems, bool acceptOutOfDateItems, bool collectionAvailable, ItemRequiredListingStatusEnum listingStatus, Guid? selectedCampaignId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RequirementListing newRequirementListing = CreateRequirementListing(db, user, itemDescription, itemType, quantityRequired, uom, requiredFrom, requiredTo, acceptDamagedItems, acceptOutOfDateItems, collectionAvailable, listingStatus, selectedCampaignId);
            db.Dispose();
            return newRequirementListing;
        }

        public static RequirementListing CreateRequirementListing(ApplicationDbContext db, IPrincipal user, string itemDescription, ItemTypeEnum itemType, decimal quantityRequired, string uom, DateTime? requiredFrom, DateTime? requiredTo, bool acceptDamagedItems, bool acceptOutOfDateItems, bool collectionAvailable, ItemRequiredListingStatusEnum listingStatus, Guid? selectedCampaignId)
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
                AcceptOutOfDateItems = acceptOutOfDateItems,
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
            return CreateRequirementListing(db, user, requirementListingAddView.ItemDescription, requirementListingAddView.ItemType, requirementListingAddView.QuantityRequired, requirementListingAddView.UoM, requirementListingAddView.RequiredFrom, requirementListingAddView.RequiredTo, requirementListingAddView.AcceptDamagedItems, requirementListingAddView.AcceptOutOfDateItems, requirementListingAddView.CollectionAvailable, requirementListingAddView.ListingStatus, requirementListingAddView.SelectedCampaignId);
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

        public static RequirementListing UpdateRequirementListingFromRequirementListingEditView(RequirementListingEditView view)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RequirementListing listing = UpdateRequirementListingFromRequirementListingEditView(db, view);
            db.Dispose();
            return listing;
        }

        public static RequirementListing UpdateRequirementListingFromRequirementListingEditView(ApplicationDbContext db, RequirementListingEditView view)
        {
            RequirementListing listing = GetRequirementListing(db, view.ListingId);
            listing.ItemDescription = view.ItemDescription;
            listing.ItemType = view.ItemType;
            listing.QuantityRequired = view.QuantityRequired;
            listing.QuantityFulfilled = view.QuantityFulfilled;
            listing.QuantityOutstanding = view.QuantityOutstanding;
            listing.UoM = view.UoM;
            listing.RequiredFrom = view.RequiredFrom;
            listing.RequiredTo = view.RequiredTo;
            listing.AcceptDamagedItems = view.AcceptDamagedItems;
            listing.AcceptOutOfDateItems = view.AcceptOutOfDateItems;
            listing.CollectionAvailable = view.CollectionAvailable;
            listing.ListingStatus = view.ListingStatus;
            listing.CampaignId = view.SelectedCampaignId;

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

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUser.AppUserId);
            Branch currentBranch = BranchHelpers.GetBranch(appUser.CurrentBranchId);

            List<RequirementListing> allRequirementListings = RequirementListingHelpers.GetAllGeneralInfoFilteredRequirementListings(db, appUser.AppUserId);

            foreach (RequirementListing requirementListing in allRequirementListings)
            {
                //Find any related offers
                Offer offer = OfferHelpers.GetOfferForListingAndUser(db, requirementListing.ListingId, appUser.AppUserId);
                decimal offerQty = 0M;
                if (offer != null)
                    offerQty = offer.CurrentOfferQuantity;

                bool userBlocked = false;
                bool branchBlocked = false;
                bool companyBlocked = false;
                
                BlockHelpers.GetBlocksForAllTypesForSpecificOfBy(db, requirementListing.ListingOriginatorAppUserId, appUser.AppUserId, requirementListing.ListingOriginatorBranchId, currentBranch.BranchId, requirementListing.ListingOriginatorCompanyId, currentBranch.CompanyId, out userBlocked, out branchBlocked, out companyBlocked);

                bool userMatchedOwner = false;
                bool branchMatchedOwner = false;
                bool companyMatchedOwner = false;

                if (currentBranch.CompanyId == requirementListing.ListingOriginatorCompanyId)
                    companyMatchedOwner = true;
                if (currentBranch.BranchId == requirementListing.ListingOriginatorBranchId)
                {
                    companyMatchedOwner = false;
                    branchMatchedOwner = true;
                }
                if (appUser.AppUserId == requirementListing.ListingOriginatorAppUserId)
                {
                    companyMatchedOwner = false;
                    branchMatchedOwner = false;
                    userMatchedOwner = true;
                }

                Company company = CompanyHelpers.GetCompany(db, requirementListing.ListingOriginatorCompanyId);

                RequirementListingGeneralInfoView requirementListingGeneralInfoView = new RequirementListingGeneralInfoView()
                {
                    RequirementListing = requirementListing,
                    OfferQuantity = offerQty,
                    AllowBranchTrading = company.AllowBranchTrading,
                    UserLevelBlock = userBlocked,
                    BranchLevelBlock = branchBlocked,
                    CompanyLevelBlock = companyBlocked,
                    DisplayBlocks = settings.RequiredListingGeneralInfoDisplayBlockedListings,
                    CompanyLevelOwner = companyMatchedOwner,
                    DisplayMyCompanyRecords = settings.RequiredListingGeneralInfoDisplayMyUserListings,
                    BranchLevelOwner = branchMatchedOwner,
                    DisplayMyBranchRecords = settings.RequiredListingGeneralInfoDisplayMyBranchListings,
                    UserLevelOwner = userMatchedOwner,
                    DisplayMyRecords = settings.RequiredListingGeneralInfoDisplayMyUserListings
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

        public static List<RequirementListingManageView> GetAllRequirementListingsManageView(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<RequirementListingManageView> list = GetAllRequirementListingsManageView(db, user);
            db.Dispose();
            return list;
        }

        public static List<RequirementListingManageView> GetAllRequirementListingsManageView(ApplicationDbContext db, IPrincipal user)
        {
            List<RequirementListingManageView> allRequirementListingsManageView = new List<RequirementListingManageView>();

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            List<RequirementListing> allRequirementListingsForUser = RequirementListingHelpers.GetAllManageListingFilteredRequirementListings(db, appUser.AppUserId);

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

    public static class RequirementListingEditHelpers
    {
        #region Get

        public static RequirementListingEditView GetRequirementListingEditView(Guid listingId, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            RequirementListingEditView view = GetRequirementListingEditView(db, listingId, user);
            db.Dispose();
            return view;
        }

        public static RequirementListingEditView GetRequirementListingEditView(ApplicationDbContext db, Guid listingId, IPrincipal user)
        {
            RequirementListing requirementListing = RequirementListingHelpers.GetRequirementListing(db, listingId);
            AppUser listingAppUser = AppUserHelpers.GetAppUser(db, requirementListing.ListingOriginatorAppUserId);
            Branch listingBranch = BranchHelpers.GetBranch(db, requirementListing.ListingOriginatorBranchId);
            Company listingCompany = CompanyHelpers.GetCompany(db, requirementListing.ListingOriginatorCompanyId);
            Campaign listingCampaign = null;
            Guid campaignId = Guid.Empty;
            string campaignName = "";
            string campaignStrapline = "";
            string campaignDescription = "";
            DateTime? campaignStartDateTime = null;
            DateTime? campaignEndDateTime = null;

            ViewButtons buttons = ViewButtonsHelpers.GetAvailableButtonsForSingleView(db, listingAppUser, listingBranch, listingCompany, user);

            if (requirementListing.CampaignId != null)
                if (requirementListing.CampaignId.Value != Guid.Empty)
                {
                    listingCampaign = CampaignHelpers.GetCampaign(db, requirementListing.CampaignId.Value);
                    campaignId = listingCampaign.CampaignId;
                    campaignName = listingCampaign.Name;
                    campaignStrapline = listingCampaign.StrapLine;
                    campaignDescription = listingCampaign.Description;
                    campaignStartDateTime = listingCampaign.CampaignStartDateTime;
                    campaignEndDateTime = listingCampaign.CampaignEndDateTime;
                }

            RequirementListingEditView view = new RequirementListingEditView()
            {
                ListingId = requirementListing.ListingId,
                ItemDescription = requirementListing.ItemDescription,
                ItemType = requirementListing.ItemType,
                QuantityRequired = requirementListing.QuantityRequired,
                QuantityFulfilled = requirementListing.QuantityFulfilled,
                QuantityOutstanding = requirementListing.QuantityOutstanding,
                UoM = requirementListing.UoM,
                RequiredFrom = requirementListing.RequiredFrom,
                RequiredTo = requirementListing.RequiredTo,
                AcceptDamagedItems = requirementListing.AcceptDamagedItems,
                AcceptOutOfDateItems = requirementListing.AcceptOutOfDateItems,
                CollectionAvailable = requirementListing.CollectionAvailable,
                ListingStatus = requirementListing.ListingStatus,
                ListingOriginatorDateTime = requirementListing.ListingOriginatorDateTime,
                ListingAppUser = listingAppUser,
                ListingBranchDetails = listingBranch,
                ListingCompanyDetails = listingCompany,
                SelectedCampaignId = campaignId,
                CampaignName = campaignName,
                CampaignStrapLine = campaignStrapline,
                CampaignDescription = campaignStrapline,
                CampaignStartDateTime = campaignStartDateTime,
                CampaignEndDateTime = campaignEndDateTime,
                Buttons = buttons
            };

            return view;
        }

        #endregion
    }
}