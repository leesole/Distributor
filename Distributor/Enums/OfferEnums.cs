using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public class OfferEnums
    {
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