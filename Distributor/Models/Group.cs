using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.GroupEnums;

namespace Distributor.Models
{
    public class Group
    {
        [Key]
        public Guid GroupId { get; set; }

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

        //references to the listing originator
        public Guid GroupOriginatorAppUserId { get; set; }
        public DateTime GroupOriginatorDateTime { get; set; }
    }
}