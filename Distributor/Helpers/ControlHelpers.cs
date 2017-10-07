using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Distributor.Enums.BranchEnums;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.ItemEnums;
using static Distributor.Enums.OfferEnums;
using static Distributor.Enums.OrderEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.Helpers
{
    public static class ControlHelpers
    {
        #region DropDowns

        public static SelectList AllCompaniesListDropDown()
        {
            return new SelectList(CompanyHelpers.GetAllCompanies(), "CompanyId", "CompanyName");
        }

        public static SelectList AllCompaniesListDropDown(Guid companyId)
        {
            return new SelectList(CompanyHelpers.GetAllCompanies(), "CompanyId", "CompanyName", companyId);
        }

        public static SelectList AllBranchesForCompanyListDropDown(Guid companyId, Guid branchId)
        {
            return new SelectList(BranchHelpers.GetBranchesForCompany(companyId), "BranchId", "BranchName", branchId);
        }

        public static SelectList AllBranchesForCompany(Guid companyId)
        {
            return new SelectList(BranchHelpers.GetBranchesForCompany(companyId), "BranchId", "BranchName");
        }

        public static SelectList AllBranchesForUserListDropDown(Guid appUserId, Guid branchId)
        {
            return new SelectList(BranchHelpers.GetBranchesForUser(appUserId), "BranchId", "BranchName", branchId);
        }

        //public static SelectList AllActiveCampaignsForUserListDropDown(Guid appUserId)
        //{
        //    return new SelectList(CampaignHelpers.GetAllCampaignsForUser(appUserId), "CampaignId", "Name");
        //}

        public static SelectList AllActiveCampaignsForUserListDropDown(Guid appUserId, Guid? selectedCampaignId)
        {
            if (selectedCampaignId == null)
                return new SelectList(CampaignHelpers.GetAllCampaignsForUser(appUserId), "CampaignId", "Name", "Select campaign");
            else
                return new SelectList(CampaignHelpers.GetAllCampaignsForUser(appUserId), "CampaignId", "Name", selectedCampaignId);
        }

        #region BranchEnums

        public static SelectList BusinessTypeEnumListDropDown()
        {
            var businessTypeEnumList = (from BusinessTypeEnum bt in Enum.GetValues(typeof(BusinessTypeEnum))
                                        select new
                                        {
                                            Id = bt,
                                            Name = EnumHelpers.GetDescription((BusinessTypeEnum)bt)
                                        });

            return new SelectList(businessTypeEnumList, "Id", "Name");
        }

        #endregion

        #region EntityEnums

        public static SelectList EntityStatusEnumListDropDown()
        {
            var entityStatusEnumList = (from EntityStatusEnum es in Enum.GetValues(typeof(EntityStatusEnum))
                                        select new
                                        {
                                            Id = es,
                                            Name = EnumHelpers.GetDescription((EntityStatusEnum)es)
                                        });

            return new SelectList(entityStatusEnumList, "Id", "Name");
        }

        #endregion

        #region ItemEnum

        public static SelectList ItemCategoryEnumListDropDown()
        {
            var itemCategoryEnumList = (from ItemCategoryEnum es in Enum.GetValues(typeof(ItemCategoryEnum))
                                        select new
                                        {
                                            Id = es,
                                            Name = EnumHelpers.GetDescription((ItemCategoryEnum)es)
                                        });

            return new SelectList(itemCategoryEnumList, "Id", "Name");
        }

        public static SelectList ItemTypeEnumListDropDown()
        {
            var itemTypeEnumList = (from ItemTypeEnum es in Enum.GetValues(typeof(ItemTypeEnum))
                                    select new
                                    {
                                        Id = es,
                                        Name = EnumHelpers.GetDescription((ItemTypeEnum)es)
                                    });

            return new SelectList(itemTypeEnumList, "Id", "Name");
        }

        public static SelectList ItemConditionEnumListDropDown()
        {
            var itemConditionEnumList = (from ItemConditionEnum es in Enum.GetValues(typeof(ItemConditionEnum))
                                         select new
                                         {
                                             Id = es,
                                             Name = EnumHelpers.GetDescription((ItemConditionEnum)es)
                                         });

            return new SelectList(itemConditionEnumList, "Id", "Name");
        }

        public static SelectList ItemRequiredListingStatusEnumListDropDown()
        {
            var itemRequiredListingStatusEnumList = (from ItemRequiredListingStatusEnum es in Enum.GetValues(typeof(ItemRequiredListingStatusEnum))
                                                     select new
                                                     {
                                                         Id = es,
                                                         Name = EnumHelpers.GetDescription((ItemRequiredListingStatusEnum)es)
                                                     });

            return new SelectList(itemRequiredListingStatusEnumList, "Id", "Name");
        }

        #endregion

        #region OfferEnums

        public static SelectList OfferStatusEnumListDropDown()
        {
            var offerStatusEnumList = (from OfferStatusEnum ur in Enum.GetValues(typeof(OfferStatusEnum))
                                       select new
                                       {
                                           Id = ur,
                                           Name = EnumHelpers.GetDescription((OfferStatusEnum)ur)
                                       });

            return new SelectList(offerStatusEnumList, "Id", "Name");
        }

        #endregion

        #region OrderEnums

        public static SelectList OrderStatusEnumListDropDown()
        {
            var orderStatusEnumList = (from OrderStatusEnum ur in Enum.GetValues(typeof(OrderStatusEnum))
                                       select new
                                       {
                                           Id = ur,
                                           Name = EnumHelpers.GetDescription((OrderStatusEnum)ur)
                                       });

            return new SelectList(orderStatusEnumList, "Id", "Name");
        }

        #endregion

        #region UserEnums

        public static SelectList UserRoleEnumListDropDown()
        {
            var userRoleEnumList = (from UserRoleEnum ur in Enum.GetValues(typeof(UserRoleEnum))
                                    select new
                                    {
                                        Id = ur,
                                        Name = EnumHelpers.GetDescription((UserRoleEnum)ur)
                                    });

            return new SelectList(userRoleEnumList, "Id", "Name");
        }

        #endregion

        #endregion
    }
}