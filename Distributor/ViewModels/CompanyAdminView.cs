using Distributor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Distributor.ViewModels
{
    public class CompanyAdminView
    {
        public Company CompanyDetails { get; set; }

        public List<Branch> RelatedBranches { get; set; }
    }
}