using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Distributor.Enums.GeneralEnums;

namespace Distributor.Models
{
    //LSLSLS NOTE - ALL SEARCHES WILL NEED TO REFERENCE THIS TABLE TO ENSURE THAT BLOCKED ITEMS ARE NOT SHOWN
    public class Block
    {
        [Key]
        public Guid BlockId { get; set; }

        [Required]
        [Display(Name = "Type of block request")]
        public LevelEnum Type { get; set; }

        [Required]
        [Display(Name = "Blocked by")]
        public Guid BlockedById { get; set; }//will hold the appUserId or CompanyId or BranchId depending on the type of the blocker

        [Required]
        [Display(Name = "User requesting block")]
        public Guid BlockedByUserId { get; set; } //will hold the appUserId of the blocker

        [Required]
        [Display(Name = "Blocked of")]
        public Guid BlockedOfId { get; set; }  //will hold the appUserId or CompanyId or BranchId depending on the type of the blocked

        [Display(Name = "Blocked date")]
        public DateTime BlockedOn { get; set; }
    }
}