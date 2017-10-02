using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class GeneralEnums
    {
        public enum ListingTypeEnum
        {
            [Description("Requirement listing")]
            [Display(Name = "Requirement listing")]
            Requirement = 0,

            [Description("Available listing")]
            [Display(Name = "Available listing")]
            Available = 1
        }

        /// <summary>
        /// Internal Search levels to be used for selecting records for ManageView and 'my' information
        /// </summary>
        public enum InternalSearchLevelEnum
        {
            [Description("User")]
            [Display(Name = "User")]
            User = 0,

            [Description("Branch")]
            [Display(Name = "Branch")]
            Branch = 1,

            [Description("Company")]
            [Display(Name = "Company")]
            Company = 2,

            [Description("Group")]
            [Display(Name = "Group")]  //These are user built closed groups
            Group = 3
        }

        /// <summary>
        /// External Search levels to be used for selecting records for GeneralInfo and other user listing information
        /// </summary>
        public enum ExternalSearchLevelEnum
        {
            [Description("All")]
            [Display(Name = "All")]
            All = 0,

            [Description("Branch")]
            [Display(Name = "Branch")]
            Branch = 1,

            [Description("Company")]
            [Display(Name = "Company")]
            Company = 2,

            [Description("Group")]
            [Display(Name = "Group")]  //These are user built closed groups
            Group = 3
        }

        /// <summary>
        /// The Level of which an ID may refer, i.e. User = AppUserId
        /// </summary>
        public enum LevelEnum
        {
            [Description("User")]
            [Display(Name = "User")]
            User = 0,

            [Description("Branch")]
            [Display(Name = "Branch")]
            Branch = 1,

            [Description("Company")]
            [Display(Name = "Company")]
            Company = 2
        }

        /// <summary>
        /// The level of privacy for the Company, Branch or User
        /// </summary>
        public enum PrivacyLevelEnum
        {
            [Description("None")]
            [Display(Name = "None")]
            None = 0,

            [Description("User")]
            [Display(Name = "User")]
            User = 1,

            [Description("Branch")]
            [Display(Name = "Branch")]
            Branch = 2,

            [Description("Company")]
            [Display(Name = "Company")]
            Company = 3
        }
    }
}