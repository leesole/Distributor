using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.UserActionEnums;

namespace Distributor.ViewModels
{
    public class UserActionView
    {
        public Guid UserActionId { get; set; }

        [Display(Name = "Task type")]
        public ActionTypeEnum ActionType { get; set; }

        [Display(Name = "Level")]
        public LevelEnum ActionLevel { get; set; }

        [Display(Name = "Description")]
        public string ActionDescription { get; set; }

        [Display(Name = "Reference")]
        public string Reference { get; set; }  //this is used to hold the key of the reference file - i.e. AppUserId for user-on-hold, BranchId for branch-on-hold etc

        [Display(Name = "Created")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Created by")]
        public string CreatedBy { get; set; }

        [Display(Name = "Status")]
        public EntityStatusEnum EntityStatus { get; set; }
    }
}