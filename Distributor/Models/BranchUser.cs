using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.Models
{
    public class BranchUser
    {
        [Key]
        public Guid BranchUserId { get; set; }

        public Guid UserId { get; set; }
        public Guid BranchId { get; set; }
        public Guid CompanyId { get; set; }
        public UserRoleEnum UserRole { get; set; } // this allows the user role to be branch specific for the user

        public EntityStatusEnum EntityStatus { get; set; }

        public EntityStatusEnum? PreviousEntityStatus { get; set; }
        public Guid? EntityStatusChangeBy { get; set; }
        public DateTime? EntityStatusChangeDate { get; set; }
    }
}