using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.Models
{
    public class GroupMember
    {
        [Key]
        public Guid GroupMemberId { get; set; }

        public Guid GroupId { get; set; }

        public Guid AppUserId { get; set; }

        public DateTime UserAddedDateTime { get; set; }
    }
}