using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class EntityEnums
    {
        public enum EntityStatusEnum
        {
            [Description("Inactive")]
            [Display(Name = "Inactive")]
            Inactive = 0,
            [Description("Active")]
            [Display(Name = "Active")]
            Active = 1,
            [Description("On hold")]
            [Display(Name = "On hold")]
            OnHold = 2,
            [Description("Closed")]
            [Display(Name = "Closed")]
            Closed = 3
        }
    }
}