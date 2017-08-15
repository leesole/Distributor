using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class EntityEnums
    {
        public enum EntityStatusEnum
        {
            [Description("Inactive")]
            Inactive = 0,
            [Description("Active")]
            Active = 1,
            [Description("On hold")]
            OnHold = 2,
            [Description("Closed)")]
            Closed = 3
        }
    }
}