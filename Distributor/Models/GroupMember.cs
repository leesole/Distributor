using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Models
{
    //THe group member will be EITHER a user, branch or company
    public class GroupMember
    {
        [Key]
        public Guid GroupMemberId { get; set; }

        public Guid GroupId { get; set; }

        [Display(Name = "Type of group")]
        public LevelEnum Type { get; set; }

        [Display(Name = "Member reference Id")]
        public Guid ReferenceId { get; set; }

        [Display(Name = "Record added by" )]
        public Guid AddedBy { get; set; }

        [Display(Name = "Record added on")]
        public DateTime AddedDateTime { get; set; }
    }
}