using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.Models
{
    //THe group member will be EITHER a user, branch or company
    public class GroupMember
    {
        [Key]
        public Guid GroupMemberId { get; set; }

        public Guid GroupId { get; set; }

        public Guid AppUserId { get; set; }

        public Guid BranchId { get; set; }

        public Guid CompanyId { get; set; }

        public DateTime UserAddedDateTime { get; set; }
    }
}