using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.ItemEnums;

namespace Distributor.ViewModels
{
    public class AvailableListingAddView : CallingFields
    {
        [Required]
        [Display(Name = "Description")]
        public string ItemDescription { get; set; }

        [Required]
        [Display(Name = "Item type")]
        public ItemTypeEnum ItemType { get; set; }

        [Display(Name = "Quantity available")]
        public decimal QuantityRequired { get; set; }

        [Display(Name = "Unit of measure")]
        public string UoM { get; set; }

        [Display(Name = "Available from")]
        public DateTime? AvailableFrom { get; set; }

        [Display(Name = "Available to")]
        public DateTime? AvailableTo { get; set; }

        [Display(Name = "Item condition")]
        public ItemConditionEnum ItemCondition { get; set; }

        [Display(Name = "Earliest display-until date")]
        public DateTime? DisplayUntilDate { get; set; }

        [Display(Name = "Earliest sell-by date")]
        public DateTime? SellByDate { get; set; }

        [Display(Name = "Earliest use-by date")]
        public DateTime? UseByDate { get; set; }

        [Display(Name = "Can deliver?")]
        public bool DeliveryAvailable { get; set; }

        [Display(Name = "Listing status")]
        public ItemRequiredListingStatusEnum ListingStatus { get; set; }
    }

    public class AvailableListingGeneralInfoView : BlocksAndOwners
    {
        public decimal OfferQuantity { get; set; }

        public AvailableListing AvailableListing { get; set; }
    }

    public class AvailableListingManageView
    {
        public AvailableListing AvailableListing { get; set; }
    }

    public class AvailableListingEditView
    {
        public Guid ListingId { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string ItemDescription { get; set; }

        [Required]
        [Display(Name = "Item type")]
        public ItemTypeEnum ItemType { get; set; }

        [Display(Name = "Quantity available")]
        public decimal QuantityRequired { get; set; }

        [Display(Name = "Quantity reserved")]
        public decimal QuantityFulfilled { get; set; }

        [Display(Name = "Quantity available")]
        public decimal QuantityOutstanding { get; set; }

        [Display(Name = "Unit of measure")]
        public string UoM { get; set; }

        [Display(Name = "Available from")]
        public DateTime? AvailableFrom { get; set; }

        [Display(Name = "Available to")]
        public DateTime? AvailableTo { get; set; }

        [Display(Name = "Item condition")]
        public ItemConditionEnum ItemCondition { get; set; }

        [Display(Name = "Earliest display-until date")]
        public DateTime? DisplayUntilDate { get; set; }

        [Display(Name = "Earliest sell-by date")]
        public DateTime? SellByDate { get; set; }

        [Display(Name = "Earliest use-by date")]
        public DateTime? UseByDate { get; set; }

        [Display(Name = "Can deliver?")]
        public bool DeliveryAvailable { get; set; }

        [Display(Name = "Listing status")]
        public ItemRequiredListingStatusEnum ListingStatus { get; set; }

        [Display(Name = "Listing submitted date")]
        public DateTime ListingOriginatorDateTime { get; set; }

        public AppUser ListingAppUser { get; set; }
        public Branch ListingBranchDetails { get; set; }
        public Company ListingCompanyDetails { get; set; }

        public ViewButtons Buttons { get; set; }
    }
}