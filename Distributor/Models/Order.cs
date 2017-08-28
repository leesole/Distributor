using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.OrderEnums;

namespace Distributor.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        [Display(Name = "Order quantity")]
        public decimal OrderQuanity { get; set; }

        [Display(Name = "Order status")]
        public OrderStatusEnum OrderStatus { get; set; }

        [Display(Name = "Order date")]
        public DateTime? OrderCreationDateTime { get; set; }

        [Display(Name = "Distribution date")]
        public DateTime? OrderDistributionDateTime { get; set; }

        [Display(Name = "Delivered date")]
        public DateTime? OrderDeliveredDateTime { get; set; }


        [Display(Name = "Collection date")]
        public DateTime? OrderCollectedDateTime { get; set; }

        [Display(Name = "Closed date")]
        public DateTime? OrderClosedDateTime { get; set; }

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