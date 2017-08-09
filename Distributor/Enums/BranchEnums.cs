using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class BranchEnums
    {
        public enum BusinessTypeEnum
        {
            [Description("Caterer")]
            Caterer = 0,
            [Description("Charity")]
            Charity = 1,
            [Description("Church")]
            Church = 2,
            [Description("Distributor")]
            Distributor = 3,
            [Description("Food bank")]
            FoodBank = 4,
            [Description("Hotelier/hostelry")]
            HotelierHostelry = 5,
            [Description("Producer")]
            Producer = 6,
            [Description("Restaurant")]
            Restaurant = 7,
            [Description("Retailer")]
            Retailer = 8,
            [Description("Supplier")]
            Supplier = 9,
            [Description("Takeaway")]
            Takeaway = 10,
            [Description("Trader")]
            Trader = 11
        }
    }
}