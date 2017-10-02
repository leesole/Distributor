using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class UserTaskEnums
    {
        public enum TaskTypeEnum
        {
            [Description("User on hold")]
            [Display(Name = "User on hold")]
            UserOnHold = 1,
            [Description("Branch on hold")]
            [Display(Name = "Branch on hold")]
            BranchOnHold = 2
        }
    }
}