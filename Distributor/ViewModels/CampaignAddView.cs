﻿using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.ViewModels
{
    public class CampaignAddView : BaseViewWithCallingFields
    {
        [Display(Name = "Campaign name")]
        public string Name { get; set; }

        [Display(Name = "Campaign strapline")]
        public string StrapLine { get; set; }

        [Display(Name = "Campaign description")]
        public string Description { get; set; }

        [Display(Name = "Poster/image")]
        public byte[] Image { get; set; }

        [Display(Name = "Poster/image location")]
        public string ImageLocation { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Campaign start date/time")]
        public DateTime? CampaignStartDateTime { get; set; }

        [Display(Name = "Campaign end date/time")]
        public DateTime? CampaignEndDateTime { get; set; }

        [Display(Name = "Location name")]
        public string LocationName { get; set; }

        [Display(Name = "Address line 1")]
        public string LocationAddressLine1 { get; set; }

        [Display(Name = "Address line 2")]
        public string LocationAddressLine2 { get; set; }

        [Display(Name = "Address line 3")]
        public string LocationAddressLine3 { get; set; }

        [Display(Name = "Address town/city")]
        public string LocationAddressTownCity { get; set; }

        [Display(Name = "Address county")]
        public string LocationAddressCounty { get; set; }

        [Display(Name = "Address postcode")]
        public string LocationAddressPostcode { get; set; }

        [Display(Name = "Telephone number")]
        public string LocationTelephoneNumber { get; set; }

        [Display(Name = "Email")]
        public string LocationEmail { get; set; }

        [Display(Name = "Contact name")]
        public string LocationContactName { get; set; }
    }
}