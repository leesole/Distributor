using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.UserActionEnums;
using static Distributor.Enums.EntityEnums;

namespace Distributor.Models
{
    public class UserAction
    {
        [Key]
        public Guid UserActionId { get; set; }

        [Required]
        [Display(Name = "Task type")]
        public ActionTypeEnum ActionType { get; set; }

        [Display(Name = "Description")]
        public string ActionDescription { get; set; }

        public Guid ReferenceKey { get; set; }  //this is used to hold the key of the reference file - i.e. AppUserId for user-on-hold, BranchId for branch-on-hold etc

        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        [Display(Name = "Status")]
        public EntityStatusEnum EntityStatus { get; set; }
    }

    public class UserActionAssignment
    {
        [Key]
        public Guid UserActionAssignmentId { get; set; }

        public Guid UserActionId { get; set; }

        public Guid AppUserId { get; set; }  //who task is assigned to

    }
}