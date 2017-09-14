using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.FriendEnums;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Models
{
    public class Friend
    {
        [Key]
        public Guid FriendId { get; set; }

        [Required]
        [Display(Name = "Type of friend request")]
        public LevelEnum Type { get; set; }

        [Required]
        [Display(Name = "Requested by")]
        public Guid RequestedById { get; set; }//will hold the appUserId or CompanyId or BranchId depending on the type of the requestor

        [Required]
        [Display(Name = "User requesting friend")]
        public Guid RequestedByUserId { get; set; }//will hold the appUserId of the requestor

        [Required]
        [Display(Name = "Requested of")]
        public Guid RequestedOfId { get; set; }  //will hold the appUserId or CompanyId or BranchId depending on the type of the requested
        

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
    }
}