using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.OrderEnums;

namespace Distributor.ViewModels
{
    public class OrderViews
    {
    }

    public class OrderManageView
    {
        public Order OrderDetails { get; set; }
    }

    public class OrderEditView
    {
        public Guid OrderId { get; set; }

        [Display(Name = "Order quantity")]
        public decimal OrderQuanity { get; set; }

        [Display(Name = "Order status")]
        public OrderStatusEnum OrderStatus { get; set; }

        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? OrderCreationDateTime { get; set; }

        [Display(Name = "Distribution date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? OrderDistributionDateTime { get; set; }

        [Display(Name = "Delivered date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? OrderDeliveredDateTime { get; set; }


        [Display(Name = "Collection date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? OrderCollectedDateTime { get; set; }

        [Display(Name = "Received date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? OrderReceivedDateTime { get; set; }

        [Display(Name = "Closed date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? OrderClosedDateTime { get; set; }

        public AppUser OrderAppUser { get; set; }
        public Branch OrderBranchDetails { get; set; }

        public Guid OfferId { get; set; }
        public AppUser OfferAppUser { get; set; }
        public Branch OfferBranchDetails { get; set; }
        public Offer OfferDetails { get; set; }

        public Guid  ListingId { get; set; }
        public AppUser ListingAppUser { get; set; }
        public Branch ListingBranchDetails { get; set; }
        public AvailableListing AvailableListingDetails { get; set; }
        public RequirementListing RequirementListingDetails { get; set; }
    }
}