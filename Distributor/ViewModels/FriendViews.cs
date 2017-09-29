using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Distributor.Enums.FriendEnums;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.ViewModels
{
    public class FriendView
    {
        public Guid FriendId { get; set; }

        [Required]
        [Display(Name = "Type of friend request")]
        public LevelEnum Type { get; set; }

        [Required]
        [Display(Name = "Requested by")]
        public string RequestedByName { get; set; }//will hold the appUserId or CompanyId or BranchId depending on the type of the requestor

        [Required]
        [Display(Name = "User requesting friend")]
        public string RequestedByUserName { get; set; }//will hold the appUserId of the requestor

        [Required]
        [Display(Name = "Requested of")]
        public string RequestedOfName { get; set; }  //will hold the appUserId or CompanyId or BranchId depending on the type of the requested


        [Display(Name = "Status")]
        public FriendStatusEnum Status { get; set; }

        [Display(Name = "Requested date")]
        public DateTime? RequestedOn { get; set; }

        [Display(Name = "Accepted date")]
        public DateTime? AccceptedOn { get; set; }

        [Display(Name = "Rejected date")]
        public DateTime? RejectedOn { get; set; }

        [Display(Name = "Closed date")]
        public DateTime? ClosedOn { get; set; }

        public Guid ClosedBy { get; set; }  //this is here so that we can see who closed/Cancelled the friendship so that we can stop the other party re-requesting

        public bool FriendedByLoggedInUser { get; set; } //use to identify those records that belong to the user viewing them
    }

    //Information built from AppUserViews:AppUserEditView
    public class FriendViewList
    {
        public List<FriendView> UserFriendListView { get; set; }

        public List<FriendView> UserBranchFriendListView { get; set; }

        public List<FriendView> UserCompanyFriendListView { get; set; }

        public string CallingUrl { get; set; }
    }
}