using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Distributor.ViewModels
{
    public class ViewButtons
    {
        public bool CompanyBlockButton { get; set; }
        public bool CompanyAddFriendButton { get; set; }
        public bool CompanyAddToGroupButton { get; set; }

        public bool BranchBlockButton { get; set; }
        public bool BranchAddFriendButton { get; set; }
        public bool BranchAddToGroupButton { get; set; }

        public bool UserBlockButton { get; set; }
        public bool UserAddFriendButton { get; set; }
        public bool UserAddToGroupButton { get; set; }
    }
}