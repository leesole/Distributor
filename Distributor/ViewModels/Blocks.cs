using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Distributor.ViewModels
{
    public class Blocks
    {
        public bool CompanyLevelBlock { get; set; }
        public bool BranchLevelBlock { get; set; }
        public bool UserLevelBlock { get; set; }
        public bool DisplayBlocks { get; set; }
    }
}