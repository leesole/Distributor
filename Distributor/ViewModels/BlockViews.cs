using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.ViewModels
{
    public class BlockView
    {
        public Guid BlockId { get; set; }

        [Display(Name = "Type of block request")]
        public LevelEnum Type { get; set; }

        [Display(Name = "Blocked by")]
        public string BlockedByName { get; set; }//will hold the appUserId or CompanyId or BranchId depending on the type of the blocker

        [Display(Name = "User requesting block")]
        public string BlockedByUserName { get; set; } //will hold the appUserId of the blocker

        [Display(Name = "Blocked of")]
        public string BlockedOfName { get; set; }  //will hold the appUserId or CompanyId or BranchId depending on the type of the blocked

        [Display(Name = "Blocked date")]
        public DateTime BlockedOn { get; set; }

        public bool BlockedByLoggedInUser { get; set; } //use to identify those records that belong to the user viewing them
    }

    //Information built from AppUserViews:AppUserEditView
    public class BlockViewList
    {
        public List<BlockView> UserBlockListView { get; set; }

        public List<BlockView> UserBranchBlockListView { get; set; }

        public List<BlockView> UserCompanyBlockListView { get; set; }

        public string CallingUrl { get; set; }
    }
}