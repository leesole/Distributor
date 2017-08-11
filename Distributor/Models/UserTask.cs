using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserEnums;
using static Distributor.Enums.UserTaskEnums;

namespace Distributor.Models
{
    public class UserTask
    {
        [Key]
        public Guid UserTaskId { get; set; }

        [Required]
        [Display(Name = "Task type")]
        public TaskTypeEnum TaskType { get; set; }

        [Display(Name = "Description")]
        public string TaskDescription { get; set; }

        public Guid ReferenceKey { get; set; }  //this is used to hold the key of the reference file - i.e. AppUserId for user-on-hold, BranchId for branch-on-hold etc

        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        [Display(Name = "Status")]
        public EntityStatusEnum EntityStatus { get; set; }
    }

    public class UserTaskAssignment
    {
        [Key]
        public Guid UserTaskAssignmentId { get; set; }

        public Guid UserTaskId { get; set; }

        public Guid AppUserId { get; set; }  //who task is assigned to

        public UserRoleEnum UserRole { get; set; }

    }
}