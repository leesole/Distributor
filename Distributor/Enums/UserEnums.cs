using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class UserEnums
    {
        public enum UserRole
        {
            Admin = 0,
            Manager = 1,
            User = 2,
            SuperUser = 3 //used as the top level of security for admin of system
        }
    }
}