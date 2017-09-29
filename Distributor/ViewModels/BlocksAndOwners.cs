using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Distributor.ViewModels
{
    public class BlocksAndOwners
    {
        public bool CompanyLevelBlock { get; set; }
        public bool BranchLevelBlock { get; set; }
        public bool UserLevelBlock { get; set; }
        public bool DisplayBlocks { get; set; }

        public bool CompanyLevelOwner { get; set; }
        public bool DisplayMyCompanyRecords { get; set; }
        public bool BranchLevelOwner { get; set; }
        public bool DisplayMyBranchRecords { get; set; }
        public bool UserLevelOwner { get; set; }
        public bool DisplayMyRecords { get; set; }

    }
}