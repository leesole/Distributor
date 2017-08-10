using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserTaskEnums;

namespace Distributor.ViewModels
{
    public class UserTaskViewModels
    {
        public class UserTask
        {
            public Guid UserTaskId { get; set; }

            public List<UserTaskAdmin> UserTaskAdminList { get; set; }
            public List<UserTaskManager> UserTaskManagerList { get; set; }

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
    }
}