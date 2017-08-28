using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.ItemEnums;

namespace Distributor.ViewModels
{
    public class RequirementListingAddView : BaseViewWithCallingFields
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
        public DateTime? RequiredFrom { get; set; }

        [Display(Name = "Required to")]
        public DateTime? RequiredTo { get; set; }

        [Display(Name = "Accept damaged items?")]
        public bool AcceptDamagedItems { get; set; }

        [Display(Name = "Can deliver?")]
        public bool DeliveryAvailable { get; set; }

        [Display(Name = "Listing status")]
        public ItemRequiredListingStatusEnum ListingStatus { get; set; }

        //other references
        public Guid? SelectedCampaignId { get; set; }
    }

    public class RequirementListingGeneralInfoView
    {
        public decimal OfferQuantity { get; set; }

        public RequirementListing RequirementListing { get; set; }
    }

    public class RequirementListingManageView
    {
        public RequirementListing RequirementListing { get; set; }
    }
}