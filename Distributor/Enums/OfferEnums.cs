using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public class OfferEnums
    {
        public enum OfferTypeEnum
        {
            [Description("Requirement listing")]
            Requriement = 0,

            [Description("Available listing")]
            Available = 1
        }

        public enum OfferStatusEnum
        {
            [Description("New offer")]
            New = 0,
            
            [Description("Accepted")]
            Accepted = 1,

            [Description("Rejected")]
            Rejected = 2,

            [Description("Returned")]
            Returned = 3
        }
    }
}