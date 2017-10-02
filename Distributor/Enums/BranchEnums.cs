using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class BranchEnums
    {
        public enum BusinessTypeEnum
        {
            [Description("Caterer")]
            [Display(Name = "Caterer")]
            Caterer = 1,
            [Description("Charity")]
            [Display(Name = "Charity")]
            Charity = 2,
            [Description("Church")]
            [Display(Name = "Church")]
            Church = 3,
            [Description("Distributor")]
            [Display(Name = "Distributor")]
            Distributor = 4,
            [Description("Food bank")]
            [Display(Name = "Food bank")]
            FoodBank = 5,
            [Description("Hotelier/hostelry")]
            [Display(Name = "Hotelier/hostelry")]
            HotelierHostelry = 6,
            [Description("Producer")]
            [Display(Name = "Producer")]
            Producer = 7,
            [Description("Restaurant")]
            [Display(Name = "Restaurant")]
            Restaurant = 8,
            [Description("Retailer")]
            [Display(Name = "Retailer")]
            Retailer = 9,
            [Description("Supplier")]
            [Display(Name = "Supplier")]
            Supplier = 10,
            [Description("Takeaway")]
            [Display(Name = "Takeaway")]
            Takeaway = 11,
            [Description("Trader")]
            [Display(Name = "Trader")]
            Trader = 12
        }
    }
}