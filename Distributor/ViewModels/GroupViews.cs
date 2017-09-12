using Distributor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Distributor.ViewModels
{
    public class GroupListView
    {
        public Group Group { get; set; }
        public List<GroupMember> Members { get; set; }
    }

    public class GroupEditView
    {
        [Display(Name = "Group name")]
        public string Name { get; set; }

        public List<GroupListView> GroupListViewsForUser { get; set; }
    }
}