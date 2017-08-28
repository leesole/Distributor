using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class GeneralEnums
    {
        public enum ListingTypeEnum
        {
            [Description("Requirement listing")]
            Requirement = 0,

            [Description("Available listing")]
            Available = 1
        }
    }
}