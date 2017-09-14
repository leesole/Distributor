using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public class UserActionEnums
    {
        public enum ActionTypeEnum
        {
            [Description("Awaiting friend request")]
            AwaitFriendRequest = 0
        }
    }
}