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

        public decimal OrderQuanity { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public DateTime OrderCreationDateTime { get; set; }
        public DateTime OrderDistributionDateTime { get; set; }
        public DateTime OrderDeliveredDateTime { get; set; }
        public DateTime OrderCollectedDateTime { get; set; }
        public DateTime OrderClosedDateTime { get; set; }

        public Guid OrderOriginatorAppUserId { get; set; }
        public Guid OrderOriginatorBranchId { get; set; }
        public Guid OrderOriginatorCompanyId { get; set; }
        public DateTime OrderOriginatorDateTime { get; set; }


        //Reference keys
        //references to the offer originator
        public Guid OfferId { get; set; }
        public Guid OfferOriginatorAppUserId { get; set; }
        public Guid OfferOriginatorBranchId { get; set; }
        public Guid OfferOriginatorCompanyId { get; set; }

        //references to the listing originator
        public Guid ListingId { get; set; }
        public Guid ListingOriginatorAppUserId { get; set; }
        public Guid ListingOriginatorBranchId { get; set; }
        public Guid ListingOriginatorCompanyId { get; set; }
    }
}