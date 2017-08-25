using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public class ItemEnums
    {
        public enum ItemCategoryEnum
        {
            [Description("Food")]
            Food = 0
        }

        //The 'Category' relates to the above ItemCategoryEnum and will be use to group the list below
        public enum ItemTypeEnum
        {
            [Category("Food")]
            [Description("Fresh")]
            Fresh = 0,
            [Category("Food")]
            [Description("Frozen")]
            Frozen = 1,
            [Category("Food")]
            [Description("Canned")]
            Canned = 2,
            [Category("Food")]
            [Description("Packet")]
            Packet = 3
        }

        public enum ItemConditionEnum
        {
            [Description("Good")]
            Good = 0,
            [Description("Slight damage")]
            Slight = 1,
            [Description("Heavy damage")]
            Heavy = 2,
            [Description("Slight dents")]
            SlightDents = 3,
            [Description("Heavy dents")]
            HeavyDents = 4,
            [Description("Repackaged in non-orignal packaging")]
            Repackaged = 5,
            [Description("Original packaging taped")]
            Taped = 6
        }

        public enum ItemRequiredListingStatusEnum
        {
            [Description("Open")]
            Open = 0,
            [Description("Partial fulfilment")]  //set when items pledged are accepted
            Partial = 1,
            [Description("Fulfilled")] //set when all required items pledged are accepted (or when the user sets to completed
            Complete = 2,
            [Description("Cancelled")] 
            Cancelled = 3,
            [Description("Expired")]  //set when time expires and not fully fulfilled
            Expired = 4
        }
    }
}