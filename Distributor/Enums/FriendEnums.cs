using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public class FriendEnums
    {
        public enum FriendStatusEnum
        {
            [Description("Requested")]
            Requested = 0,

            [Description("Accepted")]
            Accepted = 1,

            [Description("Rejected")]
            Rejected = 2,

            [Description("Closed")]
            Closed = 2
        }
    }
}