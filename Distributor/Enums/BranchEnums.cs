using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class BranchEnums
    {
        public enum BusinessType
        {
            [Description("Producer")]
            Producer = 0,
            [Description("Distributor")]
            Distributor = 1,
            [Description("Retailer")]
            Retailer = 2,
            [Description("Supplier")]
            Supplier = 3,
            [Description("Trader")]
            Trader = 4,
            [Description("Food bank")]
            FoodBank = 5,
            [Description("Church")]
            Church = 6,
            [Description("Restaurant")]
            Restaurant = 7,
            [Description("Takeaway")]
            Takeaway = 8,
            [Description("Caterer")]
            Caterer = 9,
            [Description("Hotelier/hostelry")]
            HotelierHostelry = 10,
            [Description("Charity")]
            Charity = 11
        }
    }
}