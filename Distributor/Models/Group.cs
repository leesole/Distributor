﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.Models
{
    public class Group
    {
        [Key]
        public Guid GroupId { get; set; }

        [Required]
        [Display(Name = "Group name")]
        public string Name { get; set; }

        //references to the listing originator
        public Guid GroupOriginatorAppUserId { get; set; }
        public DateTime GroupOriginatorDateTime { get; set; }
    }
}