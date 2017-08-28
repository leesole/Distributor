﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.OfferEnums;

namespace Distributor.Models
{
    public class Offer
    {
        [Key]
        public Guid OfferId { get; set; }

        public Guid ListingId { get; set; }

        [Display(Name = "Listing type")]
        public ListingTypeEnum ListingType { get; set; }

        [Display(Name = "Offer status")]
        public OfferStatusEnum OfferStatus { get; set; }

        [Display(Name = "Offer quantity")]
        public decimal CurrentOfferQuantity { get; set; }  //set to 0 if rejected or returned

        [Display(Name = "Previous offer quantity")]
        public decimal? PreviousOfferQuantity { get; set; } //set this Current offer at time of currentofferqty entry so we have something to refer to if returned

        [Display(Name = "Counter offer quantity")]
        public decimal? CounterOfferQuantity { get; set; }  //if 'returned' status with a new offer, value is here

        //references to the offer originator
        public Guid OfferOriginatorAppUserId { get; set; }
        public Guid OfferOriginatorBranchId { get; set; }
        public Guid OfferOriginatorCompanyId { get; set; }
        public DateTime OfferOriginatorDateTime { get; set; }

        public Guid? ListingOriginatorAppUserId { get; set; }
        public Guid? ListingOriginatorBranchId { get; set; }
        public Guid? ListingOriginatorCompanyId { get; set; }
        public DateTime? ListingOriginatorDateTime { get; set; }

        public Guid? OrderId { get; set; }  //if this offer is accepted then an order is made and this is the reference to that order.
        public Guid? OrderOriginatorAppUserId { get; set; }
        public Guid? OrderOriginatorBranchId { get; set; }
        public Guid? OrderOriginatorCompanyId { get; set; }
        public DateTime? OrderOriginatorDateTime { get; set; }
    }
}