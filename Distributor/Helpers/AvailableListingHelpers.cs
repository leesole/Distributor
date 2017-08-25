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
    public class AvailableListingHelpers
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

        public static AvailableListing CreateAvailableListing(IPrincipal user, string itemDescription, ItemTypeEnum itemType, decimal quantityRequired, string uom, DateTime? availableFrom, DateTime? availableTo, ItemConditionEnum itemCondition, bool collectionAvailable, ItemRequiredListingStatusEnum listingStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AvailableListing newAvailableListing = CreateAvailableListing(db, user, itemDescription, itemType, quantityRequired, uom, availableFrom, availableTo, itemCondition, collectionAvailable, listingStatus);
            db.Dispose();
            return newAvailableListing;
        }

        public static AvailableListing CreateAvailableListing(ApplicationDbContext db, IPrincipal user, string itemDescription, ItemTypeEnum itemType, decimal quantityRequired, string uom, DateTime? availableFrom, DateTime? availableTo, ItemConditionEnum itemCondition, bool collectionAvailable, ItemRequiredListingStatusEnum listingStatus)
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
                CollectionAvailable = collectionAvailable,
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
            return CreateAvailableListing(db, user, AvailableListingAddView.ItemDescription, AvailableListingAddView.ItemType, AvailableListingAddView.QuantityRequired, AvailableListingAddView.UoM, AvailableListingAddView.AvailableFrom, AvailableListingAddView.AvailableTo, AvailableListingAddView.ItemCondition, AvailableListingAddView.CollectionAvailable, AvailableListingAddView.ListingStatus);
        }

        #endregion
    }

    public static class AvailableListingGeneralInfoHelpers
    {
        #region Get

        public static List<AvailableListingGeneralInfoView> GetAllAvailableListingsGeneralInfoView()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<AvailableListingGeneralInfoView> list = GetAllAvailableListingsGeneralInfoView(db);
            db.Dispose();
            return list;
        }

        public static List<AvailableListingGeneralInfoView> GetAllAvailableListingsGeneralInfoView(ApplicationDbContext db)
        {
            List<AvailableListingGeneralInfoView> allAvailableListingsGeneralInfoView = new List<AvailableListingGeneralInfoView>();

            List<AvailableListing> allAvailableListings = AvailableListingHelpers.GetAllAvailableListings(db);

            foreach (AvailableListing AvailableListing in allAvailableListings)
            {
                AvailableListingGeneralInfoView AvailableListingGeneralInfoView = new AvailableListingGeneralInfoView()
                {
                    AvailableListing = AvailableListing
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

}