using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class OrderEnums
    {
        public enum OrderStatusEnum
        {
            [Description("New order")]
            New = 0,
            [Description("Despatched")]
            Despatched = 1,
            [Description("Delivered")]
            Delivered = 2,
            [Description("Collected")]
            Collected = 3,
            [Description("Closed")]
            Closed = 4
        }
    }
}