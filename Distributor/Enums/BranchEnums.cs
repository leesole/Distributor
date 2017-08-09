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
            Caterer = 1,
            [Description("Charity")]
            Charity = 2,
            [Description("Church")]
            Church = 3,
            [Description("Distributor")]
            Distributor = 4,
            [Description("Food bank")]
            FoodBank = 5,
            [Description("Hotelier/hostelry")]
            HotelierHostelry = 6,
            [Description("Producer")]
            Producer = 7,
            [Description("Restaurant")]
            Restaurant = 8,
            [Description("Retailer")]
            Retailer = 9,
            [Description("Supplier")]
            Supplier = 10,
            [Description("Takeaway")]
            Takeaway = 11,
            [Description("Trader")]
            Trader = 12
        }
    }
}