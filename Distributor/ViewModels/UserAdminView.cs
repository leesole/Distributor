using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.BranchEnums;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.GeneralEnums;
using static Distributor.Enums.UserEnums;

namespace Distributor.ViewModels
{
    public class UserAdminView
    {
        public Guid AppUserId { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Status")]
        public EntityStatusEnum AppUserEntityStatus { get; set; }

        [Display(Name = "Current branch")]
        public Guid CurrentBranchId { get; set; }

        [Display(Name = "Login email")]
        public string LoginEmail { get; set; }

        [Display(Name = "Privacy level")]
        public PrivacyLevelEnum PrivacyLevel { get; set; }

        public List<UserAdminRelatedBranchesView> RelatedBranches { get; set; }
    }

    public class UserAdminRelatedBranchesView
    {
        public Guid AppUserId { get; set; }
        public Guid BranchId { get; set; }
        public Guid BranchUserId { get; set; }

        public string BranchName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressTownCity { get; set; }
        public string AddressPostcode { get; set; }
        public UserRoleEnum UserRole { get; set; }

        public bool CurrentBranch { get; set; }
    }
}