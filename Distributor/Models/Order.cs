using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        //Reference keys
        public Guid OfferId { get; set; }

        public Guid ListingId { get; set; }
        public Guid OfferOriginatorAppUserId { get; set; }
        public Guid OfferOriginatorBranchId { get; set; }
        public Guid OfferOriginatorCompanyId { get; set; }
        public DateTime OfferOriginatorDateTime { get; set; }
    }
}