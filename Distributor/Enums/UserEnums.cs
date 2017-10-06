using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class UserEnums
    {
        //Do not change this order as the number values are used within Javascript
        public enum UserRoleEnum
        {
            [Description("Admin")]
            [Display(Name = "Admin")]
            Admin = 0,
            [Description("Manager")]
            [Display(Name = "Manager")]
            Manager = 1,
            [Description("User")]
            [Display(Name = "User")]
            User = 2,
            [Description("Super user")]
            [Display(Name = "Super user")]
            SuperUser = 3 //used as the top level of security for admin of system
        }
    }
}