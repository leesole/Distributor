using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.GroupEnums;

namespace Distributor.ViewModels
{
    public class GroupListView
    {
        public Group Group { get; set; }
        public List<GroupMember> Members { get; set; }
    }

    public class GroupEditView
    {
        public List<GroupListView> GroupListViewsCreatedByUser { get; set; }
        public List<GroupListView> GroupListViewsRelevantToUser { get; set; }
    }


    public class GroupAddView
    {
        [Required]
        [Display(Name = "Group name")]
        public string Name { get; set; }

        [Display(Name = "Type of group")]
        public LevelEnum Type { get; set; }

        [Display(Name = "Group visibility")]
        public GroupVisibilityEnum VisibilityLevel { get; set; }

        [Display(Name = "Group invite level")]
        public GroupInviteLevelEnum InviteLevel { get; set; }

        [Display(Name = "Group invite acceptance level")]
        public GroupInviteAcceptanceLevelEnum AcceptanceLevel { get; set; }

        public bool scratchEntry { get; set; }

        public List<GroupAddMemberView> Members { get; set; }
    }

    public class GroupAddMemberView
    {
        [Display(Name = "Selected")]
        public bool SelectedUser { get; set; }

        public Guid ReferenceId { get; set; }

        [Display(Name = "Name")]
        public string ReferenceName { get; set; }
    }
}