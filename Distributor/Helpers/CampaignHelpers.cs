using Distributor.Models;
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

        #endregion

        #region Create

        public static Campaign CreateCampaign(Campaign campaign, IPrincipal user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Campaign newCampaign = CreateCampaign(db, campaign, user);
            db.Dispose();
            return newCampaign;
        }

        public static Campaign CreateCampaign(ApplicationDbContext db, Campaign campaign, IPrincipal user)
        {
            BranchUser branchUser = BranchUserHelpers.GetBranchUserCurrentForUser(db, user);

            campaign.CampaignId = Guid.NewGuid();
            campaign.CampaignOriginatorAppUserId = branchUser.UserId;
            campaign.CampaignOriginatorBranchId = branchUser.BranchId;
            campaign.CampaignOriginatorCompanyId = branchUser.CompanyId;
            campaign.CampaignOriginatorDateTime = DateTime.Now;
            campaign.EntityStatus = EntityStatusEnum.Active;

            db.Campaigns.Add(campaign);
            db.SaveChanges();

            return campaign;
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
}