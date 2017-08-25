using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.OfferEnums;

namespace Distributor.Helpers
{
    public static class OfferHelpers
    {
        #region Get

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
                                                  where (bu.UserId == o.OfferOriginatorAppUserId && bu.BranchId == o.OfferOriginatorBranchId && bu.CompanyId == o.OfferOriginatorCompanyId
                                                  && o.OfferStatus == OfferStatusEnum.New && o.OfferStatus == OfferStatusEnum.Accepted)
                                                  select o).ToList();

            return allOffersForBranchUser;
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

                switch (offerForBranchUser.OfferType)
                {
                    case OfferTypeEnum.Available:
                        availableListing = AvailableListingHelpers.GetAvailableListing(db, offerForBranchUser.ListingId);
                        break;
                    case OfferTypeEnum.Requriement:
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

                

                //public static List<CampaignManageView> GetAllCampaignsManageViewForUserBranch(ApplicationDbContext db, IPrincipal user)
                //{
                //    List<CampaignManageView> allCampaignsManageView = new List<CampaignManageView>();

                //    BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);
                //    List<Campaign> allCampaignsForBranchUser = CampaignHelpers.GetAllCampaignsForBranchUser(db, branchUser);

                //    foreach (Campaign campaignForBranchUser in allCampaignsForBranchUser)
                //    {
                //        CampaignManageView campaignManageView = new CampaignManageView()
                //        {
                //            Campaign = campaignForBranchUser
                //        };

                //        allCampaignsManageView.Add(campaignManageView);
                //    }

                //    return allCampaignsManageView;
                //}

        
    }