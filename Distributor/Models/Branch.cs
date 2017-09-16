﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.BranchEnums;
using static Distributor.Enums.EntityEnums;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Models
{
    public class Branch
    {
        [Key]
        public Guid BranchId { get; set; }

        public Guid CompanyId { get; set; }

        [Required]
        [Display(Name = "Branch name")]
        public string BranchName { get; set; }

        [Required]
        [Display(Name = "Business type")]
        public BusinessTypeEnum BusinessType { get; set; }

        [Required]
        [Display(Name = "Address line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Address line 3")]
        public string AddressLine3 { get; set; }

        [Required]
        [Display(Name = "Address town/city")]
        public string AddressTownCity { get; set; }

        [Display(Name = "Address county")]
        public string AddressCounty { get; set; }

        [Required]
        [Display(Name = "Address postcode")]
        public string AddressPostcode { get; set; }

        [Required]
        [Display(Name = "Telephone number")]
        public string TelephoneNumber { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Contact name")]
        public string ContactName { get; set; }

        [Display(Name = "Privacy level")]
        public PrivacyLevelEnum PrivacyLevel { get; set; }

        [Display(Name = "Status")]
        public EntityStatusEnum EntityStatus { get; set; }
    }
}