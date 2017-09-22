using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Distributor.Enums
{
    public static class GroupEnums
    {
        //Groups are private at present so this is not used
        public enum GroupVisibilityEnum
        {
            [Description("Private")]
            Private = 0,
            [Description("Public")]
            Public = 1
        }

        //Level of who can invite someone to the group
        public enum GroupInviteLevelEnum
        {
            [Description("Group owner")]
            Owner = 0,
            [Description("Group member")]
            Member = 1
        }

        //Level of acceptance steps for new users
        public enum GroupInviteAcceptanceLevelEnum
        {
            [Description("Automatic acceptance")]
            Automatic = 0,
            [Description("Group member acceptance")]
            Member = 1,
            [Description("Group invitee acceptance")]
            Invitee = 2,
            [Description("Group owner acceptance")]
            Owner = 3
        }
    }
}