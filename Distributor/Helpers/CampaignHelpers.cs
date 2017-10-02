﻿using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Helpers
{
    public static class CampaignHelpers
    {
        #region Get

        public static Campaign GetCampaign(Guid campaignId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Campaign campaign = GetCampaign(db, campaignId);
            db.Dispose();
            return campaign;
        }

        public static Campaign GetCampaign(ApplicationDbContext db, Guid campaignId)
        {
            return db.Campaigns.Find(campaignId);
        }

        public static List<Campaign> GetAllCampaigns()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Campaign> list = GetAllCampaigns(db);
            db.Dispose();
            return list;
        }

        public static List<Campaign> GetAllCampaigns(ApplicationDbContext db)
        {
            return db.Campaigns.ToList();
        }
        
        public static List<Campaign> GetAllGeneralInfoFilteredCampaigns(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Campaign> list = GetAllGeneralInfoFilteredCampaigns(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Campaign> GetAllGeneralInfoFilteredCampaigns(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            var dateCheck = DateTime.MinValue;
            int settingsMaxDistance = settings.CampaignGeneralInfoMaxDistance ?? 0;

            //Create list within the time frame if setting set
            List<Campaign> list = (from c in db.Campaigns
                                   where (c.EntityStatus == EntityStatusEnum.Active && c.CampaignOriginatorDateTime >= dateCheck)
                                   orderby c.CampaignEndDateTime ?? DateTime.MaxValue
                                   select c).ToList();

            //Now bring in the Selection Level sort
            switch (settings.CampaignGeneralInfoExternalSelectionLevel)
            {
                case ExternalSearchLevelEnum.All: //do nothing
                    break;
                case ExternalSearchLevelEnum.Branch: //user's current branch to filter
                    list = list.Where(l => l.CampaignOriginatorBranchId == branch.BranchId).ToList();
                    break;
                case ExternalSearchLevelEnum.Company: //user's current company to filter
                    list = list.Where(l => l.CampaignOriginatorCompanyId == branch.CompanyId).ToList();
                    break;
                case ExternalSearchLevelEnum.Group: //user's built group sets to filter ***TO BE DONE***
                    break;
            }

            //Now sort by distance
            if (settingsMaxDistance > 0)
            {
                List<Campaign> removeList = new List<Campaign>();

                foreach (Campaign campaign in list)
                {
                    DistanceHelpers distance = new DistanceHelpers();
                    int distanceValue = distance.GetDistance(branch.AddressPostcode, campaign.LocationAddressPostcode);

                    if (distanceValue > settingsMaxDistance)
                        removeList.Add(campaign);
                }

                if (removeList.Count > 0)
                    list.RemoveAll(l => removeList.Contains(l));
            }

            return list;
        }

        public static List<Campaign> GetAllManageListingFilteredCampaigns(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Campaign> list = GetAllManageListingFilteredCampaigns(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Campaign> GetAllManageListingFilteredCampaigns(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            //Create list 
            List<Campaign> list = new List<Campaign>();

            //Now bring in the Selection Level sort
            switch (settings.CampaignManageViewInternalSelectionLevel)
            {
                case InternalSearchLevelEnum.User:
                    list = GetAllCampaignsForUser(db, appUserId);
                    break;
                case InternalSearchLevelEnum.Branch: //user's current branch to filter
                    list = GetAllCampaignsForBranch(db, branch.BranchId);
                    break;
                case InternalSearchLevelEnum.Company: //user's current company to filter
                    list = GetAllCampaignsForCompany(db, branch.CompanyId);
                    break;
                case InternalSearchLevelEnum.Group: //user's built group sets to filter ***TO BE DONE***
                    break;
            }

            return list;
        }

        public static List<Campaign> GetAllDashboardFilteredCampaigns(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Campaign> list = GetAllDashboardFilteredCampaigns(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Campaign> GetAllDashboardFilteredCampaigns(ApplicationDbContext db, Guid appUserId)
        {
            AppUser appUser = AppUserHelpers.GetAppUser(db, appUserId);
            Branch branch = BranchHelpers.GetBranch(db, appUser.CurrentBranchId);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUserId);

            var dateCheck = DateTime.MinValue;
            double settingsMaxAge = settings.CampaignDashboardMaxAge ?? 0;
            int settingsMaxDistance = settings.CampaignDashboardMaxDistance ?? 0;

            if (settingsMaxAge > 0)
            {
                double negativeDays = 0 - settingsMaxAge;
                dateCheck = DateTime.Now.AddDays(negativeDays);
            }

            //Create list within the time frame if setting set
            List<Campaign> list = (from c in db.Campaigns
                                   where (c.EntityStatus == EntityStatusEnum.Active && c.CampaignOriginatorDateTime >= dateCheck)
                                   select c).ToList();

            //Now bring in the Selection Level sort
            switch (settings.CampaignDashboardExternalSelectionLevel)
            {
                case ExternalSearchLevelEnum.All: //do nothing
                    break;
                case ExternalSearchLevelEnum.Branch: //user's current branch to filter
                    list = list.Where(l => l.CampaignOriginatorBranchId == branch.BranchId).ToList();
                    break;
                case ExternalSearchLevelEnum.Company: //user's current company to filter
                    list = list.Where(l => l.CampaignOriginatorCompanyId == branch.CompanyId).ToList();
                    break;
                case ExternalSearchLevelEnum.Group: //user's built group sets to filter ***LSLSLS TO BE DONE***
                    break;
            }

            //Now sort by distance
            if (settingsMaxDistance > 0)
            {
                List<Campaign> removeList = new List<Campaign>();

                foreach (Campaign campaign in list)
                {
                    DistanceHelpers distance = new DistanceHelpers();
                    int distanceValue = distance.GetDistance(branch.AddressPostcode, campaign.LocationAddressPostcode);

                    if (distanceValue > settingsMaxDistance)
                        removeList.Add(campaign);
                }

                if (removeList.Count > 0)
                    list.RemoveAll(l => removeList.Contains(l));
            }

            return list;
        }

        public static List<Campaign> GetAllCampaignsForUser(Guid appUserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Campaign> list = GetAllCampaignsForUser(db, appUserId);
            db.Dispose();
            return list;
        }

        public static List<Campaign> GetAllCampaignsForUser(ApplicationDbContext db, Guid appUserId)
        {
            List<Campaign> allCampaignsForUser = (from c in db.Campaigns
                                                  where (c.CampaignOriginatorAppUserId == appUserId && c.EntityStatus == EntityStatusEnum.Active)
                                                  orderby c.CampaignEndDateTime ?? DateTime.MaxValue
                                                  select c).ToList();

            return allCampaignsForUser;
        }

        public static List<Campaign> GetAllCampaignsForBranch(Guid branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Campaign> list = GetAllCampaignsForBranch(db, branchId);
            db.Dispose();
            return list;
        }

        public static List<Campaign> GetAllCampaignsForBranch(ApplicationDbContext db, Guid branchId)
        {
            List<Campaign> allCampaignsForUser = (from c in db.Campaigns
                                                  where (c.CampaignOriginatorBranchId == branchId && c.EntityStatus == EntityStatusEnum.Active)
                                                  orderby c.CampaignEndDateTime ?? DateTime.MaxValue
                                                  select c).ToList();

            return allCampaignsForUser;
        }

        public static List<Campaign> GetAllCampaignsForCompany(Guid companyId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Campaign> list = GetAllCampaignsForCompany(db, companyId);
            db.Dispose();
            return list;
        }

        public static List<Campaign> GetAllCampaignsForCompany(ApplicationDbContext db, Guid companyId)
        {
            List<Campaign> allCampaignsForUser = (from c in db.Campaigns
                                                  where (c.CampaignOriginatorCompanyId == companyId && c.EntityStatus == EntityStatusEnum.Active)
                                                  orderby c.CampaignEndDateTime ?? DateTime.MaxValue
                                                  select c).ToList();

            return allCampaignsForUser;
        }

        public static List<Campaign> GetAllCampaignsForBranchUser(BranchUser branchUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<Campaign> list = GetAllCampaignsForBranchUser(db, branchUser);
            db.Dispose();
            return list;
        }

        public static List<Campaign> GetAllCampaignsForBranchUser(ApplicationDbContext db, BranchUser branchUser)
        {
            List<Campaign> allCampaignsForBranchUser = (from bu in db.BranchUsers
                                                        from c in db.Campaigns
                                                        where (bu.UserId == c.CampaignOriginatorAppUserId && bu.BranchId == c.CampaignOriginatorBranchId && bu.CompanyId == c.CampaignOriginatorCompanyId
                                                        && c.EntityStatus == EntityStatusEnum.Active)
                                                        select c).ToList();

            return allCampaignsForBranchUser;
        }

        #endregion

        #region Create

        public static Campaign CreateCampaign(IPrincipal user, string name, string strapLine, string description, byte[] image, string imageLocation, string website, DateTime? campaignStartDateTime, DateTime? campaignEndDateTime, string locationName, string locationAddressLine1, string locationAddressLine2, string locationAddressLine3, string locationAddressTownCity, string locationAddressCounty, string locationAddressPostcode, string locationTelephoneNumber, string locationEmail, string locationContactName, EntityStatusEnum entityStatus)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Campaign campaign = CreateCampaign(db, user, name, strapLine, description, image, imageLocation, website, campaignStartDateTime, campaignEndDateTime, locationName, locationAddressLine1, locationAddressLine2, locationAddressLine3, locationAddressTownCity, locationAddressCounty, locationAddressPostcode, locationTelephoneNumber, locationEmail, locationContactName, entityStatus);

            db.Dispose();

            return campaign;
        }

        public static Campaign CreateCampaign(ApplicationDbContext db, IPrincipal user, string name, string strapLine, string description, byte[] image, string imageLocation, string website, DateTime? campaignStartDateTime, DateTime? campaignEndDateTime, string locationName, string locationAddressLine1, string locationAddressLine2, string locationAddressLine3, string locationAddressTownCity, string locationAddressCounty, string locationAddressPostcode, string locationTelephoneNumber, string locationEmail, string locationContactName, EntityStatusEnum entityStatus)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);

            Campaign campaign = new Campaign()
            {
                CampaignId = Guid.NewGuid(),
                Name = name,
                StrapLine = strapLine,
                Description = description,
                Image = image,
                ImageLocation = imageLocation,
                Website = website,
                CampaignStartDateTime = campaignStartDateTime,
                CampaignEndDateTime = campaignEndDateTime,
                LocationName = locationName,
                LocationAddressLine1 = locationAddressLine1,
                LocationAddressLine2 = locationAddressLine2,
                LocationAddressLine3 = locationAddressLine3,
                LocationAddressTownCity = locationAddressTownCity,
                LocationAddressCounty = locationAddressCounty,
                LocationAddressPostcode = locationAddressPostcode,
                LocationTelephoneNumber = locationTelephoneNumber,
                LocationEmail = locationEmail,
                LocationContactName = locationContactName,
                EntityStatus = entityStatus,
                CampaignOriginatorAppUserId = branchUser.UserId,
                CampaignOriginatorBranchId = branchUser.BranchId,
                CampaignOriginatorCompanyId = branchUser.CompanyId,
                CampaignOriginatorDateTime = DateTime.Now
            };

            db.Campaigns.Add(campaign);
            db.SaveChanges();

            return campaign;
        }

        public static Campaign CreateCampaignFromCampaignAddView(CampaignAddView campaignAddView, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Campaign newCampaign = CreateCampaignFromCampaignAddView(db, campaignAddView, user);
            db.Dispose();
            return newCampaign;
        }

        public static Campaign CreateCampaignFromCampaignAddView(ApplicationDbContext db, CampaignAddView campaignAddView, IPrincipal user)
        {
            return CreateCampaign(db, user, campaignAddView.Name, campaignAddView.StrapLine, campaignAddView.Description, campaignAddView.Image, campaignAddView.ImageLocation, campaignAddView.Website, campaignAddView.CampaignStartDateTime, campaignAddView.CampaignEndDateTime, campaignAddView.LocationName, campaignAddView.LocationAddressLine1, campaignAddView.LocationAddressLine2, campaignAddView.LocationAddressLine3, campaignAddView.LocationAddressTownCity, campaignAddView.LocationAddressCounty, campaignAddView.LocationAddressTownCity, campaignAddView.LocationTelephoneNumber, campaignAddView.LocationEmail, campaignAddView.LocationContactName, EntityStatusEnum.Active);
        }

        #endregion

        #region Update

        public static Campaign UpdateCampaignFromCampaignEditView(CampaignEditView view)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Campaign campaignDetails = UpdateCampaignFromCampaignEditView(db, view);
            db.Dispose();
            return campaignDetails;
        }

        public static Campaign UpdateCampaignFromCampaignEditView(ApplicationDbContext db, CampaignEditView view)
        {
            
            Campaign campaignDetails = GetCampaign(db, view.CampaignId);
            campaignDetails.CampaignId = view.CampaignId;
            campaignDetails.Name = view.Name;
            campaignDetails.StrapLine = view.StrapLine;
            campaignDetails.Description = view.Description;
            campaignDetails.Image = view.Image;
            campaignDetails.ImageLocation = view.ImageLocation;
            campaignDetails.Website = view.Website;
            campaignDetails.CampaignStartDateTime = view.CampaignStartDateTime;
            campaignDetails.CampaignEndDateTime = view.CampaignEndDateTime;
            campaignDetails.LocationName = view.LocationName;
            campaignDetails.LocationAddressLine1 = view.LocationAddressLine1;
            campaignDetails.LocationAddressLine2 = view.LocationAddressLine2;
            campaignDetails.LocationAddressLine3 = view.LocationAddressLine3;
            campaignDetails.LocationAddressTownCity = view.LocationAddressTownCity;
            campaignDetails.LocationAddressCounty = view.LocationAddressCounty;
            campaignDetails.LocationAddressPostcode = view.LocationAddressPostcode;
            campaignDetails.LocationTelephoneNumber = view.LocationTelephoneNumber;
            campaignDetails.LocationEmail = view.LocationEmail;
            campaignDetails.LocationContactName = view.LocationContactName;
            campaignDetails.EntityStatus = view.EntityStatus;
            if (view.CampaignOriginatorDateTime != DateTime.MinValue) 
                campaignDetails.CampaignOriginatorDateTime = view.CampaignOriginatorDateTime;

            db.Entry(campaignDetails).State = EntityState.Modified;
            db.SaveChanges();

            return campaignDetails;
        }

        #endregion
    }

    public static class CampaignGeneralInfoViewHelpers
    {
        #region Get

        public static List<CampaignGeneralInfoView> GetAllCampaignsGeneralInfoView(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<CampaignGeneralInfoView> list = GetAllCampaignsGeneralInfoView(db, user);
            db.Dispose();
            return list;
        }

        public static List<CampaignGeneralInfoView> GetAllCampaignsGeneralInfoView(ApplicationDbContext db, IPrincipal user)
        {
            List<CampaignGeneralInfoView> allCampaignsGeneralInfoView = new List<CampaignGeneralInfoView>();

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            AppUserSettings settings = AppUserSettingsHelpers.GetAppUserSettingsForUser(db, appUser.AppUserId);
            Branch currentBranch = BranchHelpers.GetBranch(appUser.CurrentBranchId);

            List<Campaign> allCampaigns = CampaignHelpers.GetAllGeneralInfoFilteredCampaigns(db, appUser.AppUserId);

            foreach (Campaign campaign in allCampaigns)
            {
                bool userBlocked = false;
                bool branchBlocked = false;
                bool companyBlocked = false;

                BlockHelpers.GetBlocksForAllTypesForSpecificOfBy(db, campaign.CampaignOriginatorAppUserId, appUser.AppUserId, campaign.CampaignOriginatorBranchId, currentBranch.BranchId, campaign.CampaignOriginatorCompanyId, currentBranch.CompanyId, out userBlocked, out branchBlocked, out companyBlocked);

                bool userMatchedOwner = false;
                bool branchMatchedOwner = false;
                bool companyMatchedOwner = false;

                if (currentBranch.CompanyId == campaign.CampaignOriginatorCompanyId)
                    companyMatchedOwner = true;
                if (currentBranch.BranchId == campaign.CampaignOriginatorBranchId)
                {
                    companyMatchedOwner = false;
                    branchMatchedOwner = true;
                }
                if (appUser.AppUserId == campaign.CampaignOriginatorAppUserId)
                {
                    companyMatchedOwner = false;
                    branchMatchedOwner = false;
                    userMatchedOwner = true;
                }

                CampaignGeneralInfoView campaignGeneralInfoView = new CampaignGeneralInfoView()
                {
                    Campaign = campaign,
                    UserLevelBlock = userBlocked,
                    BranchLevelBlock = branchBlocked,
                    CompanyLevelBlock = companyBlocked,
                    DisplayBlocks = settings.CampaignGeneralInfoDisplayBlockedListings,
                    CompanyLevelOwner = companyMatchedOwner,
                    DisplayMyCompanyRecords = settings.CampaignGeneralInfoDisplayMyUserListings,
                    BranchLevelOwner = branchMatchedOwner,
                    DisplayMyBranchRecords = settings.CampaignGeneralInfoDisplayMyBranchListings,
                    UserLevelOwner = userMatchedOwner,
                    DisplayMyRecords = settings.CampaignGeneralInfoDisplayMyUserListings
                };

                allCampaignsGeneralInfoView.Add(campaignGeneralInfoView);
            }

            return allCampaignsGeneralInfoView;
        }

        #endregion
    }

    public static class CampaignManageHelpers
    {
        #region Get

        public static List<CampaignManageView> GetAllCampaignsManageView(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<CampaignManageView> list = GetAllCampaignsManageView(db, user);
            db.Dispose();
            return list;
        }

        public static List<CampaignManageView> GetAllCampaignsManageView(ApplicationDbContext db, IPrincipal user)
        {
            List<CampaignManageView> allCampaignsManageView = new List<CampaignManageView>();

            AppUser appUser = AppUserHelpers.GetAppUser(db, user);
            List<Campaign> allCampaignsForUser = CampaignHelpers.GetAllManageListingFilteredCampaigns(db, appUser.AppUserId);

            foreach (Campaign campaignForBranchUser in allCampaignsForUser)
            {
                CampaignManageView campaignManageView = new CampaignManageView()
                {
                    Campaign = campaignForBranchUser
                };

                allCampaignsManageView.Add(campaignManageView);
            }

            return allCampaignsManageView;
        }

        public static List<CampaignManageView> GetAllCampaignsManageViewForUserBranch(IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<CampaignManageView> list = GetAllCampaignsManageViewForUserBranch(db, user);
            db.Dispose();
            return list;
        }

        public static List<CampaignManageView> GetAllCampaignsManageViewForUserBranch(ApplicationDbContext db, IPrincipal user)
        {
            List<CampaignManageView> allCampaignsManageView = new List<CampaignManageView>();

            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);
            List<Campaign> allCampaignsForBranchUser = CampaignHelpers.GetAllCampaignsForBranchUser(db, branchUser);

            foreach (Campaign campaignForBranchUser in allCampaignsForBranchUser)
            {
                CampaignManageView campaignManageView = new CampaignManageView()
                {
                    Campaign = campaignForBranchUser
                };

                allCampaignsManageView.Add(campaignManageView);
            }

            return allCampaignsManageView;
        }

        #endregion
    }

    public static class CampaignEditHelpers
    {
        #region Get

        public static CampaignEditView GetCampaignEditView(Guid campaignId, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            CampaignEditView view = GetCampaignEditView(db, campaignId, user);
            db.Dispose();
            return view;
        }

        public static CampaignEditView GetCampaignEditView(ApplicationDbContext db, Guid campaignId, IPrincipal user)
        {
            Campaign campaignDetails = CampaignHelpers.GetCampaign(db, campaignId);
            AppUser campaignAppUser = AppUserHelpers.GetAppUser(db, campaignDetails.CampaignOriginatorAppUserId);
            Branch campaignBranch = BranchHelpers.GetBranch(db, campaignDetails.CampaignOriginatorBranchId);
            Company campaignCompany = CompanyHelpers.GetCompany(db, campaignDetails.CampaignOriginatorCompanyId);

            ViewButtons buttons = ViewButtonsHelpers.GetAvailableButtonsForSingleView(db, campaignAppUser, campaignBranch, campaignCompany, user);

            CampaignEditView view = new CampaignEditView()
            {
                CampaignId = campaignDetails.CampaignId,
                Name = campaignDetails.Name,
                StrapLine = campaignDetails.StrapLine,
                Description = campaignDetails.Description,
                Image = campaignDetails.Image,
                ImageLocation = campaignDetails.ImageLocation,
                Website = campaignDetails.Website,
                CampaignStartDateTime = campaignDetails.CampaignStartDateTime,
                CampaignEndDateTime = campaignDetails.CampaignEndDateTime,
                LocationName = campaignDetails.LocationName,
                LocationAddressLine1 = campaignDetails.LocationAddressLine1,
                LocationAddressLine2 = campaignDetails.LocationAddressLine2,
                LocationAddressLine3 = campaignDetails.LocationAddressLine3,
                LocationAddressTownCity = campaignDetails.LocationAddressTownCity,
                LocationAddressCounty = campaignDetails.LocationAddressCounty,
                LocationAddressPostcode = campaignDetails.LocationAddressPostcode,
                LocationTelephoneNumber = campaignDetails.LocationTelephoneNumber,
                LocationEmail = campaignDetails.LocationEmail,
                LocationContactName = campaignDetails.LocationContactName,
                EntityStatus = campaignDetails.EntityStatus,
                CampaignOriginatorDateTime = campaignDetails.CampaignOriginatorDateTime,
                CampaignAppUser = campaignAppUser,
                CampaignBranchDetails = campaignBranch,
                CampaignCompanyDetails = campaignCompany,
                Buttons = buttons
            };

            return view;
        }

        #endregion
    }
}