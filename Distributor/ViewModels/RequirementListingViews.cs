﻿using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.ItemEnums;

namespace Distributor.ViewModels
{
    public class RequirementListingAddView : CallingFields
    {
        [Required]
        [Display(Name = "Description")]
        public string ItemDescription { get; set; }

        [Required]
        [Display(Name = "Item type")]
        public ItemTypeEnum ItemType { get; set; }

        [Display(Name = "Quantity required")]
        public decimal QuantityRequired { get; set; }

        [Display(Name = "Unit of measure")]
        public string UoM { get; set; }

        [Display(Name = "Required from")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? RequiredFrom { get; set; }

        [Display(Name = "Required to")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? RequiredTo { get; set; }

        [Display(Name = "Accept damaged items?")]
        public bool AcceptDamagedItems { get; set; }

        [Display(Name = "Accept out-of-date items?")]
        public bool AcceptOutOfDateItems { get; set; }

        [Display(Name = "Can collect?")]
        public bool CollectionAvailable { get; set; }

        [Display(Name = "Listing status")]
        public ItemRequiredListingStatusEnum ListingStatus { get; set; }

        //other references
        [Display(Name = "Assign to a campaign")]
        public Guid? SelectedCampaignId { get; set; }
    }

    public class RequirementListingGeneralInfoView : BlocksAndOwners
    {
        public decimal OfferQuantity { get; set; }

        public bool AllowBranchTrading { get; set; }

        public RequirementListing RequirementListing { get; set; }
    }

    public class RequirementListingManageView
    {
        public RequirementListing RequirementListing { get; set; }
    }

    public class RequirementListingEditView
    {
        public Guid ListingId { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string ItemDescription { get; set; }

        [Required]
        [Display(Name = "Item type")]
        public ItemTypeEnum ItemType { get; set; }

        [Display(Name = "Quantity required")]
        public decimal QuantityRequired { get; set; }

        [Display(Name = "Quantity fulfilled")]
        public decimal QuantityFulfilled { get; set; }

        [Display(Name = "Quantity needed")]
        public decimal QuantityOutstanding { get; set; }

        [Display(Name = "Unit of measure")]
        public string UoM { get; set; }

        [Display(Name = "Required from")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? RequiredFrom { get; set; }

        [Display(Name = "Required to")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? RequiredTo { get; set; }

        [Display(Name = "Accept damaged items?")]
        public bool AcceptDamagedItems { get; set; }

        [Display(Name = "Accept out-of-date items?")]
        public bool AcceptOutOfDateItems { get; set; }

        [Display(Name = "Can collect?")]
        public bool CollectionAvailable { get; set; }

        [Display(Name = "Listing status")]
        public ItemRequiredListingStatusEnum ListingStatus { get; set; }

        [Display(Name = "Listing submitted date")]
        public DateTime ListingOriginatorDateTime { get; set; }

        //other references
        [Display(Name = "Assigned to a campaign")]
        public Guid? SelectedCampaignId { get; set; }

        [Display(Name = "Campaign name")]
        public string CampaignName { get; set; }

        [Display(Name = "Campaign strapline")]
        public string CampaignStrapLine { get; set; }

        [Display(Name = "Campaign description")]
        public string CampaignDescription { get; set; }

        [Display(Name = "Campaign start date/time")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? CampaignStartDateTime { get; set; }

        [Display(Name = "Campaign end date/time")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? CampaignEndDateTime { get; set; }

        public AppUser ListingAppUser { get; set; }
        public Branch ListingBranchDetails { get; set; }
        public Company ListingCompanyDetails { get; set; }
        //public Campaign CampaignDetails { get; set; }

        public ViewButtons Buttons { get; set; }
    }
}