using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public class ItemEnums
    {
        public enum ItemCategoryEnum
        {
            [Description("Food")]
            [Display(Name = "Food")]
            Food = 0
        }

        //The 'Category' relates to the above ItemCategoryEnum and will be use to group the list below
        public enum ItemTypeEnum
        {
            [Category("Food")]
            [Description("Fresh")]
            [Display(Name = "Fresh")]
            Fresh = 0,
            [Category("Food")]
            [Description("Frozen")]
            [Display(Name = "Frozen")]
            Frozen = 1,
            [Category("Food")]
            [Description("Canned")]
            [Display(Name = "Canned")]
            Canned = 2,
            [Category("Food")]
            [Description("Packet")]
            [Display(Name = "Packet")]
            Packet = 3,
            [Category("Food")]
            [Description("Cooked")]
            [Display(Name = "Cooked")]
            Cooked = 4,
            [Category("Food")]
            [Description("Carton")]
            [Display(Name = "Carton")]
            Carton = 5,
            [Category("Food")]
            [Description("Prepared / Microwave")]
            [Display(Name = "Prepared / Microwave")]
            PreparedMicrowave = 6
        }

        public enum ItemConditionEnum
        {
            [Description("Good")]
            [Display(Name = "Good")]
            Good = 0,
            [Description("Slight damage")]
            [Display(Name = "Slight damage")]
            Slight = 1,
            [Description("Heavy damage")]
            [Display(Name = "Heavy damage")]
            Heavy = 2,
            [Description("Slight dents")]
            [Display(Name = "Slight dents")]
            SlightDents = 3,
            [Description("Heavy dents")]
            [Display(Name = "Heavy dents")]
            HeavyDents = 4,
            [Description("Repackaged in non-orignal packaging")]
            [Display(Name = "Repackaged in non-orignal packaging")]
            Repackaged = 5,
            [Description("Original packaging taped")]
            [Display(Name = "Original packaging taped")]
            Taped = 6
        }

        public enum ItemRequiredListingStatusEnum
        {
            [Description("Open")]
            [Display(Name = "Open")]
            Open = 0,
            [Description("Partial fulfilment")]
            [Display(Name = "Partial fulfilment")]  //set when items pledged are accepted
            Partial = 1,
            [Description("Fulfilled")]
            [Display(Name = "Fulfilled")] //set when all required items pledged are accepted (or when the user sets to completed
            Complete = 2,
            [Description("Cancelled")]
            [Display(Name = "Cancelled")] 
            Cancelled = 3,
            [Description("Expired")]
            [Display(Name = "Expired")]  //set when time expires and not fully fulfilled
            Expired = 4
        }
    }
}