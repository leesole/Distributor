using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.ItemEnums;

namespace Distributor.Models
{
    public class RequirementListing
    {
        [Key]
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
        public DateTime? RequiredFrom { get; set; }

        [Display(Name = "Required to")]
        public DateTime? RequiredTo { get; set; }

        [Display(Name = "Accept damaged items?")]
        public bool AcceptDamagedItems { get; set; }
        
        [Display(Name = "Can collect?")]
        public bool CollectionAvailable { get; set; }

        [Display(Name = "Listing status")]
        public ItemRequiredListingStatusEnum ListingStatus { get; set; }

        [Display(Name = "Listing location")]
        public string ListingBranchPostcode { get; set; }  //Put here for quicker sorting in view screens

        //references to the listing originator
        public Guid ListingOriginatorAppUserId { get; set; }
        public Guid ListingOriginatorBranchId { get; set; }
        public Guid ListingOriginatorCompanyId { get; set; }
        public DateTime ListingOriginatorDateTime { get; set; }

        //other references
        public Guid? CampaignId { get; set; }
    }
}