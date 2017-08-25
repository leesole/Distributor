﻿using Distributor.Models;
using Distributor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using static Distributor.Enums.EntityEnums;

namespace Distributor.Helpers
{
    public static class CampaignHelpers
    {
        #region Get

        public static Campaign GetCmapaign(Guid campaignId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Campaign campaign = GetCmapaign(db, campaignId);
            db.Dispose();
            return campaign;
        }

        public static Campaign GetCmapaign(ApplicationDbContext db, Guid campaignId)
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
    }

    public static class CampaignGeneralInfoViewHelpers
    {
        #region Get

        public static List<CampaignGeneralInfoView> GetAllCampaignsGeneralInfoView()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<CampaignGeneralInfoView> list = GetAllCampaignsGeneralInfoView(db);
            db.Dispose();
            return list;
        }

        public static List<CampaignGeneralInfoView> GetAllCampaignsGeneralInfoView(ApplicationDbContext db)
        {
            List<CampaignGeneralInfoView> allCampaignsGeneralInfoView = new List<CampaignGeneralInfoView>();

            List<Campaign> allCampaigns = CampaignHelpers.GetAllCampaigns(db);

            foreach (Campaign campaign in allCampaigns)
            {
                CampaignGeneralInfoView campaignGeneralInfoView = new CampaignGeneralInfoView()
                {
                    Campaign = campaign
                };

                allCampaignsGeneralInfoView.Add(campaignGeneralInfoView);
            }

            return allCampaignsGeneralInfoView;
        }

        #endregion
    }

    public static class CampaignGeneralManageHelpers
    {
        #region Get

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
}