using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.BranchEnums;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserEnums;
using static Distributor.Enums.UserTaskEnums;

namespace Distributor.ViewModels
{
    public class UserTaskView
    {
        public Guid UserTaskId { get; set; }

        [Display(Name = "Task type")]
        public TaskTypeEnum TaskType { get; set; }

        [Display(Name = "Description")]
        public string TaskDescription { get; set; }

        public AppUser AppUserReference { get; set; }
        public Branch BranchReference { get; set; }

        [Display(Name = "Created")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Created by")]
        public AppUser CreatedBy { get; set; }

        [Display(Name = "Status")]
        public EntityStatusEnum EntityStatus { get; set; }
    }

    public class UserTaskFullView
    {
        public UserTaskView UserTaskView { get; set; }

        //to be used for the new User
        [Display(Name ="User branch role")]
        public UserRoleEnum? BranchUserUserRole { get; set; }

        public AppUser CreatedByAppUser { get; set; }

        public Branch CreatedByAppUserCurrentBranch { get; set; }
    }
}