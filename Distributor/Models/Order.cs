﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.OrderEnums;

namespace Distributor.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        [Display(Name = "Listing type")]
        public ListingTypeEnum ListingType { get; set; }

        [Display(Name = "Order quantity")]
        public decimal OrderQuanity { get; set; }

        [Display(Name = "Order status")]
        public OrderStatusEnum OrderStatus { get; set; }

        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrderCreationDateTime { get; set; }

        [Display(Name = "Distribution date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrderDistributionDateTime { get; set; }
        public Guid? OrderDistributedBy { get; set; }

        [Display(Name = "Delivered date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrderDeliveredDateTime { get; set; }
        public Guid? OrderDeliveredBy { get; set; }


        [Display(Name = "Collection date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrderCollectedDateTime { get; set; }
        public Guid? OrderCollectedBy { get; set; }

        [Display(Name = "Received date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrderReceivedDateTime { get; set; }
        public Guid? OrderReceivedBy { get; set; }

        [Display(Name = "Closed date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrderClosedDateTime { get; set; }
        public Guid? OrderClosedBy { get; set; }

        public Guid? OrderOriginatorAppUserId { get; set; }
        public Guid? OrderOriginatorBranchId { get; set; }
        public Guid? OrderOriginatorCompanyId { get; set; }
        public DateTime? OrderOriginatorDateTime { get; set; }


        //Reference keys
        //references to the offer originator
        public Guid? OfferId { get; set; }
        public Guid? OfferOriginatorAppUserId { get; set; }
        public Guid? OfferOriginatorBranchId { get; set; }
        public Guid? OfferOriginatorCompanyId { get; set; }

        //references to the listing originator
        public Guid? ListingId { get; set; }
        public Guid? ListingOriginatorAppUserId { get; set; }
        public Guid? ListingOriginatorBranchId { get; set; }
        public Guid? ListingOriginatorCompanyId { get; set; }
    }
}