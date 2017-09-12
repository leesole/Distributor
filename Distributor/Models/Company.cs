using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.EntityEnums;

namespace Distributor.Models
{
    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; }

        public Guid HeadOfficeBranchId { get; set; }

        [Required]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company registration details")]
        public string CompanyRegistrationDetails { get; set; }

        [Display(Name = "Charity registration details")]
        public string CharityRegistrationDetails { get; set; }

        [Display(Name = "VAT registration details")]
        public string VATRegistrationDetails { get; set; }

        [Display(Name = "Allow in-house trading between branches?")]
        public bool AllowBranchTrading { get; set; }

        [Display(Name = "Status")]
        public EntityStatusEnum EntityStatus { get; set; }
    }
}