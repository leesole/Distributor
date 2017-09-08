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

        /// <summary>
        /// Internal Search levels to be used for selecting records for ManageView and 'my' information
        /// </summary>
        public enum InternalSearchLevelEnum
        {
            [Description("User")]
            User = 0,

            [Description("Branch")]
            Branch = 1,

            [Description("Company")]
            Company = 2,

            [Description("Group")]  //These are user built closed groups
            Group = 3
        }

        /// <summary>
        /// External Search levels to be used for selecting records for GeneralInfo and other user listing information
        /// </summary>
        public enum ExternalSearchLevelEnum
        {
            [Description("All")]
            All = 0,

            [Description("Branch")]
            Branch = 1,

            [Description("Company")]
            Company = 2,

            [Description("Group")]  //These are user built closed groups
            Group = 3
        }
    }
}