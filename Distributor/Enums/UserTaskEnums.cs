using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class UserTaskEnums
    {
        public enum TaskTypeEnum
        {
            [Description("User on hold")]
            UserOnHold = 1,
            [Description("Branch on hold")]
            BranchOnHold = 2
        }
    }
}