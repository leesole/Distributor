using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class UserEnums
    {
        public enum UserRoleEnum
        {
            [Description("Admin")]
            Admin = 0,
            [Description("Manager")]
            Manager = 1,
            [Description("User")]
            User = 2,
            [Description("Super user")]
            SuperUser = 3 //used as the top level of security for admin of system
        }
    }
}