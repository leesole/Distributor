﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Models
{
    public class AppUser
    {
        [Key]
        public Guid AppUserId { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Status")]
        public EntityStatusEnum EntityStatus { get; set; }

        [Display(Name = "Current branch")]
        public Guid CurrentBranchId { get; set; }

        [Display(Name = "Login email")]
        public string LoginEmail { get; set; }

        [Display(Name = "Privacy level")]
        public PrivacyLevelEnum PrivacyLevel { get; set; }

        [Display(Name = "Admin user?")]
        public bool AdminUser { get; set; }

        [Display(Name = "Super user?")]
        public bool SuperUser { get; set; }
    }
}